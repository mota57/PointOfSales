

namespace PointOfSales.Core.Infraestructure.VueTable
{
    ///https://www.laravel-enso.com/examples/table

    public class VueTableParameters
    {
        public string Query { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
        public string OrderBy { get; set; }
        public string Ascending { get; set; }
        public int ByColumn { get; set; }
    }
}
