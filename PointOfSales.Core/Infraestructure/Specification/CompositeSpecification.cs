using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointOfSales.Core.Infraestructure.Specification
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public Dictionary<string, List<string>> ErrorList = new Dictionary<string, List<string>>();

        public abstract void Run(T candidate);
        public bool IsValid()
        {
            return ErrorList.Count == 0;
        }

        public void AddError(string message,  string key = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException(nameof(message));
            }

            if (ErrorList.ContainsKey(key))
            {
                ErrorList[key].Add(message);
            }
            else
            {
                ErrorList.Add(key, new List<string>() { message });
            }
        }

      


        public IEnumerable<ValidationResult> GetMessageErrors()
        {
            var validationResults = new List<ValidationResult>();
            var tempValues = ErrorList.ToList();
            ErrorList.Clear();
            tempValues.ForEach(er =>
            {
                foreach (var val in er.Value)
                {
                    validationResults.Add(new ValidationResult(val, new[] { er.Key }));
                }
            });
            return validationResults;
        }



    }

    

 }
