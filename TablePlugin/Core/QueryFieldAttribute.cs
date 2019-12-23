﻿using System;

namespace TablePlugin
{
    public class QueryFieldAttribute : System.Attribute
    {
        public QueryFieldAttribute(string name, bool filter = true, bool sort = true, bool display = true, string friendlyName = null, string type = "")
        {
            if (name.IsBlank()) throw new ArgumentNullException();
            Name = name;
            IsFilter = filter;
            IsSort = sort;
            Display = display;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public bool IsFilter { get; set; }
        public bool IsSort { get; set; }
        public bool Display { get; set; }
        public string Type { get; set; }
    }


}
