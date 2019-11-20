using System.Collections.Generic;
using System.Linq;

namespace PointOfSales.Core.Infraestructure.VueTable
{
    public class VueTableConfigDTO
    {
        public VueTableConfigDTO(VueTableConfig config)
        {
            this.TableName = config.TableName;
            this.Fields = config.Fields.Select(v => new VueFieldDTO(v));

        }

        public string TableName { get; }
        public IEnumerable<VueFieldDTO> Fields { get; }
    }
}
