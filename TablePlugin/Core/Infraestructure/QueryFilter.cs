namespace TablePlugin.Core
{
    public class QueryFilter
    {
        public QueryFilter(){
            
        }
        public QueryFilter(string name, OperatorType @operator, object value)
        {
            Name = name;
            Operator = @operator;
            Value = value;
        }

        public string Name {get;set;}
        public OperatorType? Operator {get;set;} 
       
        public string DateLogicalOperator {get;set;}
        public object Value {get;set;}
    }


}