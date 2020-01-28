using SqlKata;
using System;

namespace TablePlugin.Core
{
    public class VueFilterByColumnStrategy : IFilterByColumnStrategy
    {
        public class VueDateValueObject
        {
            public DateTime Start { get; set; }

            public DateTime End { get; set; }
             
        }

        public void FilterByColumn(Query query, QueryConfig queryConfig, IRequestParameterAdapter parameter)
        {
            var queryFilters = parameter.Query;

            foreach (QueryFilter prop in queryFilters)
            {
                if (prop.Value == null || prop.Value.ToString().IsBlank()) continue;

                var name = prop.Name;
                var value = prop.Value;
                var propConfig = queryConfig.GetQueryFieldByName(name);
                if (propConfig.Type == typeof(DateTime))
                {
                    //OnConstruction....
                    //var date = ((JObject)value).ToObject<VueDateValueObject>();
                    //query
                    //    .OrWhereDate(name, ">=", date.Start.ToString("mm/dd/yyyy"))
                        
                    //    .OrWhereDate(name, "<=", date.End.ToString("yyyy-mm-dd"));
                }
                else
                {
                    query.OrWhereLike(name, $"%{value}%");
                }
            }
        }
    }
}
