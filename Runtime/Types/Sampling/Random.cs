using System;

namespace AlephVault.Unity.Support.Generic
{
    namespace Types
    {
        namespace Sampling
        {
            /// <summary>
            ///   Given an array of elements, this object samples
            ///   a random element of it, each time it is requested
            ///   to (it is: uniformly random with replacement).
            /// </summary>
            /// <typeparam name="T">The type of elements to sample</typeparam>
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
