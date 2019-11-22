using System.Collections.Generic;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure.VueTable;

namespace PointOfSales.WebUI.Models
{
    public class DiscountVueDataTableConfig : VueTableConfig
    {
        public DiscountVueDataTableConfig()
        : base(
           "Discounts",

            new List<VueField>() {
                 new VueField(nameof(Discount.Id), false),
                 new VueField(nameof(Discount.Name)),
                 new VueField(nameof(Discount.Amount))
            }
        )
        { }
    }
}
