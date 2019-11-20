using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SqlKata;

namespace PointOfSales.Core.Infraestructure.VueTable
{
    public class VueTableConfig
    {

        public VueTableConfig(string tableName, List<VueField> fields, Query queryBuilder = null)
        {
            
            this.TableName = tableName;
            this.Fields = fields;

            
            //Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(this.TableName), "Table Name is required");
            //Contract.Requires<ArgumentNullException>(fields != null && fields.Count != 0, "Fields must at least contain a field");
            

            if(queryBuilder != null )
            {
                //Contract.Requires<ArgumentNullException>(fields.All(_ => !string.IsNullOrEmpty(_.SqlField)), 
                //    "Must set SqlField property in class VueField when VueTableConfig.QueryBuilder is not null");
            }
            QueryBuilder = queryBuilder;
        }

        /// <summary>
        ///  Must set SqlField property when VueTableConfig.QueryBuilder is not null. 
        /// </summary>
     
        public Query QueryBuilder { get;  private set;}


        public string TableName { get; private set; }
        public List<VueField> Fields { get; private set; } 
        
    }
}
