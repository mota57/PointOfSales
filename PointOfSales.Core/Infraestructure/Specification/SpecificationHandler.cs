using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.Infraestructure.Specification
{
    public class SpecificationHandler<T>
    {
        protected Dictionary<string, List<string>> ErrorList = new Dictionary<string, List<string>>();

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
                spec.Run(candidate);
                if (!spec.IsValid())
                {
                    CombineErrors(spec);
                }
            }
        }


        private void CombineErrors(CompositeSpecification<T> spec)
        {
            foreach(var err in spec.ErrorList)
            {
                if (!ErrorList.ContainsKey(err.Key))
                {
                    ErrorList.Add(err.Key, new List<string>());
                }

                ErrorList[err.Key].AddRange(err.Value);
            }
        }

        public List<ValidationResult> GetMessageErrors()
        {
            return contexts;
        }
    }
}
