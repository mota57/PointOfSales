using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointOfSales.Core.Infraestructure.Rule
{
    public class SpecificationRuleHandler<T>
    {
        private readonly SpecificationRule<T>[] specifications;

        public SpecificationRuleHandler(params SpecificationRule<T>[] specifications)
        {
            this.specifications = specifications;
        }
        public void RunSpecifications(T candidate)
        {
            foreach(var spec in specifications)
            {
                spec.Run(candidate);
            }
        }

        public IEnumerable<ValidationResult> GetErrosForMvc()
            => this.specifications.SelectMany(p => p.GetMessageErrorsForMvc());

    }
}
