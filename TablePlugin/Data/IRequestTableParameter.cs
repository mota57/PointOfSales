namespace TablePlugin.Data
{
    public interface IRequestTableParameter
    {
        int PerPage { get; set; }
        PropertyOrder[] OrderBy { get; set; }
        int Page { get; set; }
        string Query { get; set; }
        bool IsFilterByColumn { get; set; }
    }


}