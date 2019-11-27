using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PointOfSales.Core.Infraestructure.Rule
{
    public interface IRule<T>
    {
        void Run(T candidate);
    }


 
}
