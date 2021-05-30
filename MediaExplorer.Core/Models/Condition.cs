using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    class Condition
    {
        public enum Operation { And, Or, Not, None };

        private Dictionary<Operation, bool> StartValues = new Dictionary<Operation, bool>()
        {
            { Operation.And, true },
            { Operation.Or, false }
        };

        private Dictionary<Operation, Func<bool, bool, bool>> Operations = new Dictionary<Operation, Func<bool, bool, bool>>()
        {
            { Operation.And, (i1, i2) => { return i1 & i2; } },
            { Operation.Or, (i1, i2) => { return i1 | i2; } }
        };

        public List<Condition> Conditions { get; set; }
        public object Object { get; set; }

        private Operation _operation;

        private Condition(List<Condition> conditions, object o, Operation operation)
        {
            Conditions = conditions;
            Object = o;
            _operation = operation;
        }

        public bool Evaluate(Func<object, bool> expression)
        {
            if(Conditions.Count > 0)
            {
                bool result = StartValues[_operation];
                foreach(Condition condition in Conditions)
                {
                    result = Operations[_operation](result, condition.Evaluate(expression));
                }
                return result;
            }
            else
            {
                return (bool)expression?.Invoke(Object);
            }
        }
    }
}
