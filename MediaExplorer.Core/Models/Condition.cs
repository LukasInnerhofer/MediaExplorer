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

        public ObservableCollection<Condition> Conditions { get; set; }
        public object Object { get; set; }

        private Operation _op;
        public Operation Op
        { 
            get { return _op; } 
            set
            {
                _op = value;
                switch(_op)
                {
                    case Operation.And:
                    case Operation.Or:
                        Object = null;
                        break;
                    case Operation.Not:
                    case Operation.None:
                        if(Object == null)
                        {
                            Object = _createObject.Invoke();
                        }
                        break;
                }
            }
        }

        private Func<object> _createObject;

        private Condition(ICollection<Condition> conditions, object o, Operation operation, Func<object> createObject)
        {
            Conditions = new ObservableCollection<Condition>(new ObservableCollection<Condition>(conditions));
            Object = o;
            Op = operation;
            _createObject = createObject;
        }

        public Condition(Func<object> createObject) : this(new List<Condition>(), null, Operation.And, createObject)
        {

        }

        public bool Evaluate(Func<object, bool> expression)
        {
            if(Conditions.Count > 0)
            {
                bool result = StartValues[Op];
                foreach(Condition condition in Conditions)
                {
                    result = Operations[Op](result, condition.Evaluate(expression));
                    // TODO: Handle invalid operation
                }
                return result;
            }
            else
            {
                if(Op == Operation.None)
                {
                    return (bool)expression?.Invoke(Object);
                }
                if (Op == Operation.Not)
                {
                    return !(bool)expression?.Invoke(Object);
                }
                // TODO: throw InvalidOperationException
                return false;
            }
        }
    }
}
