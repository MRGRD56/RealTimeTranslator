using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeTranslator.Utils
{
    public class ValueRef<T>
    {
        private T _value;

        public T Value { get => _value; set => _value = value; }

        public ValueRef(T value)
        {
            Value = value;
        }
    }
}
