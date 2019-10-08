using System;

namespace PointOfSales.Core.Infraestructure
{
    public class FormLayoutAttribute : Attribute
    {
        public FormLayoutAttribute(string layoutName = null, params string[] fieldOrder  )
        {
            if (string.IsNullOrEmpty(layoutName))
            {
                layoutName = "default";
            }

        }
    }
}