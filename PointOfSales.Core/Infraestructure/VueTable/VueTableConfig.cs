using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SqlKata;

namespace PointOfSales.Core.Infraestructure.VueTable
{

    public class VueTableConfig
    {

        public VueTableConfig(string tableName = null, List<VueField> fields = null, Query queryBuilder = null)
        {
            
            this.TableName = tableName;
            this.Fields = fields;


            //Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(this.TableName), "Table Name is required");

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            if (fields == null || fields.Count == 0)
                throw new ArgumentNullException("fields");

            //Contract.Requires<ArgumentNullException>(fields != null && fields.Count != 0, "Fields must at least contain a field");


            if(queryBuilder != null )
            {
                if(fields.Any(_ => string.IsNullOrEmpty(_.SqlField)))
                    throw new ApplicationException("Must set SqlField property in class VueField when VueTableConfig.QueryBuilder is not null");
            //    Contract.Requires<ArgumentNullException>(fields.All(_ => !string.IsNullOrEmpty(_.SqlField)), 
            //        "Must set SqlField property in class VueField when VueTableConfig.QueryBuilder is not null");
            }
            QueryBuilder = queryBuilder;
        }

        /// <summary>
        ///  Must set SqlField property when VueTableConfig.QueryBuilder is not null. 
        /// </summary>
     
        public Query QueryBuilder { get;  set;}


        public string TableName { get; private set; }
        public List<VueField> Fields { get; private set; } 
        
    }
}
