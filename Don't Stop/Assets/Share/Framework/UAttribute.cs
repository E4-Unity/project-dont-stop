using System;

namespace Framework
{
    public abstract class UAttributeBase
    {
        public static UAttributeBase operator +(UAttributeBase _left, UAttributeBase _right)
        {
            return _left.Add(_right);
        }

        protected abstract UAttributeBase Add(UAttributeBase _other);
    }
    
    public abstract class UAttribute<T> : UAttributeBase where T : UAttribute<T>, new()
    {
        public static T operator +(UAttribute<T> _left, UAttributeBase _right)
        {
            return ((UAttributeBase)_left + _right) as T;
        }
    }
}
