using System.Collections.Generic;

namespace TablePlugin.Core
{
    public class DataResponse<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
