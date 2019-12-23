namespace TablePlugin.Core
{
    public class QueryRecordAttribute : System.Attribute
    {
        public QueryRecordAttribute(string configName, string tableName)
        {
            ConfigName = configName;
            TableName = tableName;
        }

        public string ConfigName { get; set; }
        public string TableName { get; set; }
    }

}
