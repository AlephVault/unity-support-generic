using System;

namespace AlephVault.Unity.Support.Generic
{
    namespace Types
    {
        namespace Sampling
        {
            public class Random<T>
            {
                private static Random random = new Random();
                private T[] source;

                public Random(T[] elements)
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
                    return source[random.Next(source.Length)];
                }
            }
        }
    }
}
