using System.Collections.Generic;
using System;

namespace TablePlugin.Data
{
    public interface IRequestTableParameter
    {
        int PerPage { get; set; }
        PropertyOrder[] OrderBy { get; set; }
        int Page { get; set; }
        ICollection<QueryFilter> Query { get; set; } // QueryFilter[] {Name, Operator, Value}
        bool IsFilterByColumn { get; set; }
    }

  
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




     public enum OperatorType 
     {
        
        StartWith,
        EndWith,
        Contains, 
        
        Equals,
        NotEquals,

        LessThan,
        LessOrEqual,
        GreaterThan,
        GreaterOrEqual,
        DateWithoutTime
    }


}