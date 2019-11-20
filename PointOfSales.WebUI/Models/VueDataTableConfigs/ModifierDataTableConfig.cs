using System.Collections.Generic;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure.VueTable;
using SqlKata;
using SqlKata.Extensions;

namespace PointOfSales.WebUI.Models
{
    public class ModifierDataTableConfig : VueTableConfig
    {
        public ModifierDataTableConfig()
        : base(
            nameof(Modifier),
            new List<VueField>()
            {
                 new VueField(name:"Id", sqlField:"Modifier.Id" ),
                 new VueField(name:"Name", sqlField:"Modifier.Name"),
                 new VueField(name:"ModifierCount", sqlField:"ModifierCount"),
            },

            new Query(nameof(Modifier))
            .Select("Modifier.Id", "Modifier.Name")
            .ForSqlite(q => q.SelectRaw("(Select (COUNT(Name) || ' Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount"))
            .ForSqlServer(q => q.SelectRaw("(Select (COUNT(Name) + ' Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount"))
        ) { }
    }

}
