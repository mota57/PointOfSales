using SqlKata;

namespace TablePlugin.Core
{
    public class BasicFilterByColumnStrategy : IFilterByColumnStrategy
    {
        public void FilterByColumn(Query query, QueryConfig queryConfig, IRequestParameterAdapter parameter)
        {

            var queryFilters = parameter.Query;

            foreach (QueryFilter prop in queryFilters)
            {
                if (prop.Value == null || prop.Value.ToString().IsBlank()) continue;
                var name = prop.Name;
                var value = prop.Value;


                switch (prop.Operator)
                {
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

                    //case OperatorType.DateWithoutTime:
                    //    query.OrWhereBetween(name, prop.DateLogicalOperator, value);
                    //    break;

                    default:
                        query.OrWhereContains(name, value.ToString());
                        break;

                }
            }
        }

    }
}
