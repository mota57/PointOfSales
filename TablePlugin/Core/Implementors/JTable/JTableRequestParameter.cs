using System;
using System.Collections.Generic;
using System.Text;

namespace TablePlugin.Core.Implementors.JTable
{

    public class JTableRequestParameter
    {
        public int jtStartIndex { get; set; }
        public int jtPageSize { get; set; }
        public string jtSorting { get; set; }

        public ICollection<QueryFilter> Query { get; set;}
    }

 

    public class JTableRequestParameterAdapter : IRequestParameterAdapter
    {

        public JTableRequestParameter Request { get; }

        public JTableRequestParameterAdapter(JTableRequestParameter request)
        {
            Request = request;
        }
    
        public int PerPage
        {
            get
            {
                return Request.jtPageSize;
            }
            set => throw new NotImplementedException();
        }
     
        public int Page
        {
            get {
                return Request.jtStartIndex;
            }
            set => throw new NotImplementedException();
        }
        public bool IsFilterByColumn { get; set; } = true;

        public PropertyOrder[] OrderBy
        {
            get {
                if (Request.jtSorting.IsBlank()) return null;
                var jtSorting = Request.jtSorting;
                var jtSortingArray = jtSorting.Split(' ');
                var propertyName = jtSortingArray[0];
                var sortType = jtSortingArray[1] == "ASC" ? OrderType.ASC : OrderType.DESC;

                return new PropertyOrder[] {
                    new PropertyOrder(propertyName, sortType)
                };
            }
            set => throw new NotImplementedException();
        }

        public ICollection<QueryFilter> Query
        {
            get => Request.Query;
            set => throw new NotImplementedException();
        }
      
    }


    public class JTableDataResponse<TData> : DataResponseAbstract<TData>
    {
        public JTableDataResponse(DataResponse<TData> dataResponse)
            : base(dataResponse)
        {
            Records = dataResponse.Data;
            TotalRecordCount = dataResponse.Count;
        }

        public string Result { get; set; }
        public IEnumerable<TData> Records { get; }
        public int TotalRecordCount { get;  }
    }

    public class JTableQueryPaginator<TResult> : BaseQueryPaginatorAbstract<JTableDataResponse<TResult>, TResult>
    {
        public JTableQueryPaginator(QueryConfig queryConfig) : base(queryConfig)
        {
        }
    }
}
