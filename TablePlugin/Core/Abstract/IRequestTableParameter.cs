using System.Collections.Generic;
using System;

namespace TablePlugin.Core
{
    public interface IRequestParameterAdapter
    {
        int PerPage { get; set; }
        PropertyOrder[] OrderBy { get; set; }
        int Page { get; set; }
        ICollection<QueryFilter> Query { get; set; } // QueryFilter[] {Name, Operator, Value}
        bool IsFilterByColumn { get; set; }
    }


}