using System.Collections.Generic;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure.VueTable;

namespace PointOfSales.WebUI.Models
{
    public class CategoryVueDataTableConfig : VueTableConfig
    {
        public CategoryVueDataTableConfig()
        :base(
            nameof(Category), 

            new List<VueField>() {
                 new VueField("Id", false),
                 new VueField("Name")
            }
        ) { }
    }
}
