using LiteDB;

namespace TablePlugin.Data
{
    public class QueryFieldDocument
    {
        [BsonId(true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public bool IsFilter { get; set; }
        public bool IsSort { get; set; }
        public bool Display { get; set; }
        public string Type { get; set; }
    }

}
