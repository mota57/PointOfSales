namespace PointOfSales.Core.Infraestructure.VueTable
{
    public class VueFieldDTO
    {
        public VueFieldDTO(VueField vueField)
        {
            this.Name = vueField.Name;
            this.Filter = vueField.Filter;
            this.Display = vueField.Display;
        }

        public string Name { get; set; }
        public bool Filter { get; set; }
        public bool Display { get; set; }

    }
}
