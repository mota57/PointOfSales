using System;

namespace PointOfSales.Core.Generator
{
    /// <summary>
    /// Specify the  
    /// </summary>
    public class FormLayoutAttribute : Attribute
    {
        public FormLayoutAttribute(string layoutName = null, params string[] fieldOrder  )
        {
            if(fieldOrder == null  && fieldOrder.Length == 0){
                throw new ApplicationException("must specify at least one field");
            }

            if (string.IsNullOrEmpty(layoutName))
            {
                layoutName = "default";
            }

        }
    }
}