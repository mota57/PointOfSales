using System;

namespace TablePlugin.Core
{
    public interface IQueryField
    {
        string Name { get; set; }
        string FriendlyName { get; set; }
        bool IsFilter { get; set; }
        bool IsSort { get; set; }
        bool Display { get; set; }
        Type Type { get; set; }
        
    }

}