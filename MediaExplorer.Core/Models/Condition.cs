using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.Models
{
    public class Condition
    {
        public enum Operation { And, Or, Not, None };

        public static ReadOnlyDictionary<string, Operation> OperationNameMap = new ReadOnlyDictionary<string, Operation>(new Dictionary<string, Operation>()
        {
            { "And", Operation.And },
            { "Or", Operation.Or },
            { "Not", Operation.Not },
            { "None", Operation.None }
        });

        public static IReadOnlyList<string> OperationNames => OperationNameMap.Keys.ToList();

        private Dictionary<Operation, bool> StartValues = new Dictionary<Operation, bool>()
        {
            { Operation.And, true },
            { Operation.Or, false }
        };

        private Dictionary<Operation, Func<bool, bool, bool>> Operations = new Dictionary<Operation, Func<bool, bool, bool>>()
        {
            { Operation.And, (i1, i2) => { return i1 && i2; } },
            { Operation.Or, (i1, i2) => { return i1 || i2; } }
        };

        public ReadOnlyObservableCollection<Condition> Conditions { get; set; }
        public object Object { get; set; }

        public Operation Op { get; set; }

        public Condition(ICollection<Condition> conditions, object o, Operation operation)
        {
            Conditions = new ReadOnlyObservableCollection<Condition>(new ObservableCollection<Condition>(conditions));
            Object = o;
            Op = operation;
        }

        public bool Evaluate(Func<object, bool> expression)
        {
            if(Conditions.Count > 0)
            {
                bool result = StartValues[Op];
                foreach(Condition condition in Conditions)
                {
                    result = Operations[Op](result, condition.Evaluate(expression));
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
