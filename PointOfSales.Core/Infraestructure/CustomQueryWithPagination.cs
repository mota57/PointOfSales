using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Extensions;
using PointOfSales.Core.Infraestructure.VueTable;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSales.Core.Infraestructure
{
    public class RequestTableParameter
    {
        public string Query
        {
            get;
            set;
        }
        public int Page
        {
            get;
            set;
        } = 1;
        public PropertyOrder[] OrderBy { get; set; }  
    }

    public enum OrderType { ASC, DESC }

    public class PropertyOrder
    {
        public PropertyOrder()
        {

        }

        public PropertyOrder(string propertyName, OrderType order)
        {
            ProperyName = propertyName;
            OrderType = order;

        }
        public string ProperyName { get; set; }
        public  OrderType OrderType { get;set; }
    }

    public class CustomQueryConfig
    {
        public CustomQueryConfig(string tableName, params QueryField[] fields)
        {
            if (tableName.IsBlank()) throw new ArgumentNullException();
            if (fields.Length == 0) throw new ArgumentException("fields must contain at least one value");
            TableName = tableName;
            Fields = fields;
           
        }

        public string TableName { get; }
        public QueryField[] Fields { get; set; }
        private Query query = null;
        protected internal Query Query
        {
            get
            {
                if(query == null)
                {
                    query = new Query(TableName).Select(Fields.Select(p => p.Name).ToArray());
                }
                return query;
            }
        } 
        public string ConnectionString { get; set; }
        public Entities.DatabaseProvider Provider { get; set; }
    }



    public class QueryField
    {
        public QueryField(string name, bool filter = true, bool sort = true, bool display = true)
        {
            if (name.IsBlank()) throw new ArgumentNullException();
            Name = name;
            IsFilter = filter;
            IsSort = sort;
            Display = display;
        }

        public string Name { get; }
        public bool IsFilter { get; }
        public bool IsSort { get; }
        public bool Display { get; }
    }

    public class CustomQueryWithPagination 
    {

        public CustomQueryWithPagination()
        {

        }


        private const int DEFAULT_PER_PAGE = 15;
        private int _perPage = DEFAULT_PER_PAGE;


        public int PerPage {
            get { return _perPage; }
            set {
                if(value < 0)
                    _perPage = DEFAULT_PER_PAGE;
                 else
                    _perPage = value;
            }
        }


        public async Task<object> GetAsync(CustomQueryConfig queryConfig, RequestTableParameter parameter)
        {
            Query query = queryConfig.Query.Clone();

            QueryFactory db = QueryFactoryBuilder.BuildQueryFactory(queryConfig);

            FilterByColumn(query, queryConfig, parameter.Query);
            OrderBy(query, parameter.OrderBy);
            Paginate(query, parameter.Page);

            var count = await db.FromQuery(query).CountAsync<int>();
            var data = await db.FromQuery(query).GetAsync<object>();

            return new 
            {
                data,
                count 
            };
        }

        private void OrderBy(Query query, PropertyOrder[] propertiesOrder)
        {
            if (propertiesOrder == null) return;

            foreach(var item in propertiesOrder)
            {
                if (item.OrderType == OrderType.ASC)
                    query = query.OrderBy(item.ProperyName);
                else
                    query = query.OrderByDesc(item.ProperyName);
            }
        }

        private void Paginate(Query queryBuilder, int page)
              => queryBuilder = queryBuilder.ForPage(page, PerPage);


        private void FilterByColumn(Query query, CustomQueryConfig queryConfig, string queryString)
        {
            if (queryString.IsBlank()) return;

            JObject obj = JsonConvert.DeserializeObject<JObject>(queryString);
            foreach (JProperty prop in obj.Properties())
            {
                CheckValidProperty(prop.Name, queryConfig);

                if (!prop.HasValues) continue;
                var name = prop.Name;
                var value = prop.Value.ToString();
                query = query.OrWhereLike(name, $"%{value}%");
            }

        }

        private void CheckValidProperty(string prop, CustomQueryConfig config)
        {
            if (!config.Fields.Where(f => f.IsFilter).Any(p => p.Name.EqualIgnoreCase(prop)))
                throw new ArgumentException($"field name {prop} doesn't exists");
        }
    }
}
