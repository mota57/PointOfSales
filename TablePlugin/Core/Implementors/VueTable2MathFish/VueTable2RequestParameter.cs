namespace TablePlugin.Core
{
    public class VueTable2RequestParameter
    {
        public string Query { get; set; }
        public int Limit { get; set; }
        public int Ascending { get; set; }
        public int ByColumn { get; set; }
        public int Page { get; set; }
    }
}