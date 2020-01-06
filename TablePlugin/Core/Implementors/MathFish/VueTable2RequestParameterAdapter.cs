using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using TablePlugin.Data;

namespace TablePlugin.Core
{

    public class VueTable2RequestParameterAdapter : IRequestParameter
    {
        private readonly VueTable2RequestParameter vueTable;

        public VueTable2RequestParameterAdapter(VueTable2RequestParameter vueTable)
        {
            this.vueTable = vueTable;
        }

   
        public PropertyOrder[] OrderBy { get; set; } = new PropertyOrder[] { };
        public int Page {
            get => vueTable.Page;
            set => vueTable.Page = value;
        }
        public ICollection<QueryFilter> Query
        {
            get {

                try
                {
                    //for the default filters
                    JObject obj = JsonConvert.DeserializeObject<JObject>(vueTable.Query);
                    var records= new List<QueryFilter>();
                    foreach (var prop in obj.Properties())
                    {
                        records.Add(new QueryFilter(prop.Name, OperatorType.Contains, prop.Value));
                    }
                    return records;

                } catch (InvalidCastException) {

                    return JsonConvert.DeserializeObject<List<QueryFilter>>(vueTable.Query);

                } catch (Exception ex)
                {
                    throw ex;
                }
            } 

            set => vueTable.Query = JsonConvert.SerializeObject(value);
        }

        
        public bool IsFilterByColumn
        {
            get => vueTable.ByColumn == 1;
            set
            {
                vueTable.ByColumn = (value) ? 1 : 0;
            }
        }

        public int PerPage { get => vueTable.Limit; set => vueTable.Limit = value; }
    }


}