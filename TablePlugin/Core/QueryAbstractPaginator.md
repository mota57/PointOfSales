```
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TablePlugin.Data;

namespace TablePlugin.Core
{
     public interface IQueryPaginator
     {
         QueryConfig QueryConfig {get; set;} 
         IRequestTableParameter Parameter {get; set;}
         Task<object> GetAsync();
         Task<DataResponse<TData>> GetAsync<TData>();
     }

     public IRequestTableParameterJTable : IRequestTableParameter
     {
         ///implement it differently.
     }

     public abstract class QueryPaginatorAbstract : IQueryPaginator
     {
         public QueryPaginatorAbstract(QueryConfig queryConfig)
         {
             this.QueryConfig = queryConfig;
         }

         public int PerPage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public QueryConfig QueryConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         
		 
         public async Task<object> GetAsync(IRequestTableParameter parameter)
         {
             ProcessQuery(parameter);
             return await BuildResponseAsync<object>();
         }

         public  async Task<DataResponse<TData>> GetAsync<TData>(IRequestTableParameter parameter)
         {
             ProcessQuery(parameter);
             return await BuildResponseAsync<TData>();
         }

         private void ProcessQuery(IRequestTableParameter parameter)
         {
             Filter(parameter);
             OrderBy(parameter);
             Paginate(parameter);
         }

         protected abstract void Paginate();
         protected abstract void OrderBy();
         protected abstract void Filter();

        
         public abstract Task<DataResponse<TData>> BuildResponseAsync<TData>();

     }

     public class QueryPaginatorLITEDB : QueryPaginatorAbstract
     {
         public QueryPaginatorLITEDB(QueryConfig queryConfig) : base(queryConfig)
         {
         }

         public override Task<DataResponse<TData>> BuildResponseAsync<TData>()
         {
             throw new NotImplementedException();
         }

         protected override void Filter(IRequestTableParameter parameter)
         {
             FilterByColumn(parameter)
         }


		 private void FilterByColumn( IRequestTableParameter parameter)
        {
            var queryString = parameter.Query;
            if (parameter.IsFilterByColumn == false || queryString.IsBlank()) return;

            QueryFilter[] obj = JsonConvert.DeserializeObject<QueryFilter[]>(queryString);
            foreach (QueryFilter prop in obj)
            {
                if (prop.Value == null || prop.Value.ToString().IsBlank()) continue;
                var name = prop.Name;
                var value = prop.Value;

                switch (prop.Operator)
                {
                    case OperatorType.Contains:
                        query.OrWhereContains(name, value.ToString());
                        break;

                    case OperatorType.EndWith:
                        query.OrWhereEnds(name, value.ToString());
                        break;

                    case OperatorType.StartWith:
                        query.OrWhereStarts(name, value.ToString());
                        break;


                    case OperatorType.Equals:
                        query.OrWhere(name, "=", value);
                        break;

                    case OperatorType.NotEquals:
                        query.OrWhere(name, "!=", value);
                        break;

                    case OperatorType.LessThan:
                        query.OrWhere(name, "<", value);
                        break;


                    case OperatorType.LessOrEqual:
                        query.OrWhere(name, "<=", value);
                        break;


                    case OperatorType.GreaterThan:
                        query.OrWhere(name, ">", value);
                        break;


                    case OperatorType.GreaterOrEqual:
                        query.OrWhere(name, ">=", value);
                        break;

                    case OperatorType.DateWithoutTime:
                        query.OrWhereDate(name, prop.DateLogicalOperator, value);
                    break;
                }
            }
        }


         protected override void OrderBy(IRequestTableParameter parameter)
         {
             throw new NotImplementedException(IRequestTableParameter parameter);
         }

         protected override void Paginate(IRequestTableParameter parameter)
         {
             throw new NotImplementedException();
         }
     }
}
```