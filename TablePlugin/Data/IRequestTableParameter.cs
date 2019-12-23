namespace TablePlugin.Data {
    public interface IRequestTableParameter
    {
        PropertyOrder[] OrderBy { get; set; }
        int Page { get; set; }
        string Query { get; set; }
        bool IsFilterByColumn { get; set; }
    }

    
    public class PropertyOrder
    {
        public PropertyOrder()
        {

        }

        public PropertyOrder(string propertyName, OrderType order)
        {
            ProperyName = propertyName;
            OrderType = order;

        }
        public string ProperyName { get; set; }
        public OrderType OrderType { get; set; }
    }

    public enum OrderType { ASC, DESC }


}