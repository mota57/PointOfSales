using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointOfSales.Core.Infraestructure.Rule
{
    public abstract class SpecificationRule<T> : IRule<T>
    {
        public Dictionary<string, List<string>> ErrorList = new Dictionary<string, List<string>>();

        public abstract void Run(T candidate);
        public bool IsValid()
        {
            return ErrorList.Count == 0;
        }

        public void AddError(string message,  string key = "")
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException(nameof(message));

            if (ErrorList.ContainsKey(key))
                ErrorList[key].Add(message);
            else
                ErrorList.Add(key, new List<string>() { message });
        }


        public IEnumerable<ValidationResult> GetMessageErrorsForMvc()
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

        // private List<ConditionKey<T>> conditionSet = new List<ConditionKey<T>>();
        //public ConditionKey<T> RuleFor<T>(string key)
        //{
        //   var instance = new ConditionKey<T>(key);
        //   conditionSet.Add(instance);
        //   return instance; 
        //}
        // RunValidations => conditionSet.ForEach(d => if(d.callback(candidate)) {  errLis.AddErr(d.Key, d.message)})
    }


    //public class ConditionKey<T>
    //{
    //    public ConditionKey(string key)
    //    {
    //        Key = key;
    //    }

    //    public string Key { get; private set; }
    //    public List<ConditionItem<T>> ConditionItems { get; private set; }

    //    public ConditionKey<T> SetCondition(string message, Func<T, bool> condition)
    //    {
    //        ConditionItems.Add(new ConditionItem<T>(message, condition));
    //        return this;
    //    }

    //}
    //public class ConditionItem<T>
    //{
    //    public ConditionItem(string message, Func<T, bool> callBack)
    //    {
    //        Message = message;
    //        CallBack = callBack;
    //    }

    //    public bool IsValid {get; private set; }
    //    public string Message { get; }
    //    public Func<T, bool> CallBack { get; }
    // }

    // 
    //}



}
