using System;

namespace Framework
{
    public abstract class UAttribute<T> where T : UAttribute<T>, new()
    {
        // 연산자 오버로딩은 추상화할 수 없으므로 별도의 추상 함수(Add) 활용  
        public static T operator +(UAttribute<T> _a, T _b)
        {
            return _a.Add(_b);
        }

        public abstract T Add(T _other);
    }
}
