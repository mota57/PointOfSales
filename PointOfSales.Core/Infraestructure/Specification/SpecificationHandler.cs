using PointOfSales.Core.Infraestructure.Specification;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.Infraestructure.Specification
{
    public class SpecificationHandler<T>
    {
        private readonly CompositeSpecification<T>[] specifications;
        private List<ValidationResult> contexts = new List<ValidationResult>();

        public SpecificationHandler(params CompositeSpecification<T>[] specifications)
        {
            this.specifications = specifications;
        }
        public void RunSpecifications(T candidate)
        {
            foreach(var spec in specifications)
            {
                spec.IsSatisfiedBy(candidate);
                contexts.AddRange(spec.GetMessageErrors());
            }
        }

        public List<ValidationResult> GetMessageErrors()
        {
            return contexts;
        }
    }
}
