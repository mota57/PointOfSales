using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PointOfSales.Core.Infraestructure.Specification
{
    public interface ISpecification<T>
    {
        void Run(T candidate);
    }


 
}
