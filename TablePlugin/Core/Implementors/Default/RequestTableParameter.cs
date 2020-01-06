using System.Collections.Generic;
using TablePlugin.Data;

namespace TablePlugin.Core
{
    public class RequestTableParameter : IRequestParameter
    {
        public ICollection<QueryFilter> Query { get; set; }
        public int Page { get; set; } = 1;
        public PropertyOrder[] OrderBy { get; set; }
        public bool IsFilterByColumn { get; set; } = true;
        public int PerPage { get; set; }
    }
}
