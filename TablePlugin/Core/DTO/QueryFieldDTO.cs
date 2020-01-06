namespace TablePlugin.Core
{
    public class QueryFieldDTO
    {
        public QueryFieldDTO(IQueryField queryField)
        {
            this.Name = queryField.Name;
            this.Filter = queryField.IsFilter;
            this.Display = queryField.Display;
            this.Type = queryField.Type?.Name;
        }

        public string Name { get; set; }
        public bool Filter { get; set; }
        public bool Display { get; set; }

        public string Type { get; set; }

    }
}
