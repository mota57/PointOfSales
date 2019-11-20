using System.Collections.Generic;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure.VueTable;

using SqlKata;

namespace PointOfSales.WebUI.Models
{
    public class ProductDataTableConfig : VueTableConfig
    {
        public ProductDataTableConfig()
        : base(
                nameof(Product),


                new List<VueField>()
                {
                     new VueField(name:"Id", sqlField:"Product.Id"),
                     new VueField(name:"Name", sqlField:"Product.Name"),
                     new VueField(name:"Price", sqlField:"Product.Price"),
                     new VueField(name:"ProductCode", sqlField:"Product.ProductCode"),
                     new VueField(name:"CategoryName", sqlField:"Category.Name")
                },


                new Query(nameof(Product))
                .LeftJoin("Category", "Category.Id", "Product.CategoryId")
                .Select("Product.{Id,Name,Price,ProductCode}", "Category.Name as CategoryName")
        ) { }
    }
}
