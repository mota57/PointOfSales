using System;

namespace TablePlugin.Core
{
    public class QueryTargetAttribute : System.Attribute
    {
        public string Name { get; private set; }

        public QueryTargetAttribute(string name)
        {
            if (name.IsBlank()) throw new ArgumentNullException("name");
            Name = name;
        }
    }
}
