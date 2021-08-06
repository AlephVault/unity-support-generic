using System;

namespace AlephVault.Unity.Support.Generic
{
    namespace Types
    {
        namespace Sampling
        {
            public class Cyclic<T>
            {
                private T[] source;
                private int index = 0;

                public Cyclic(T[] elements)
                {
                    if (elements == null)
                    {
                        throw new ArgumentNullException("elements");
                    }
                    if (elements.Length == 0)
                    {
                        throw new ArgumentException("elements cannot be empty");
                    }
                    source = elements;
                }

                public T Get()
                {
                    T current = source[index];
                    index += 1;
                    if (index == source.Length) index = 0;
                    return current;
                }
            }
        }
    }
}
