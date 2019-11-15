using System;

namespace PointOfSales.Core.Infraestructure.VueTable
{
    public class VueField
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter"></param>
        /// <param name="sqlField">Must set SqlField property in class VueField when using inline query is not null</param>
        public VueField(string name,  bool filter = true, string sqlField = "")
        {
            if (string.IsNullOrEmpty(name)) {
                throw new Exception("field must have a name");
            }
            this.Name = name;
            this.Filter = filter;
            this.SqlField = sqlField;
        }

        public string Name { get; set; }
        public bool Filter { get; set; } = true;
        public bool Display { get; set; } = true;
        /// <summary>
        /// Must set SqlField property when VueTableConfig.QueryBuilder is not null.  
        /// </summary>
        public string SqlField { get; set; }
    }
}
