namespace TablePlugin.Core
{

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


}