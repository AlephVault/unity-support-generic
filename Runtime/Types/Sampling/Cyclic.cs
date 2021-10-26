using System;

namespace AlephVault.Unity.Support.Generic
{
    namespace Types
    {
        namespace Sampling
        {
            /// <summary>
            ///   Given an array of elements, this object samples
            ///   a sequential element of it, starting over when
            ///   reaching the end, each time it is requested to.
            /// </summary>
            /// <typeparam name="T">The type of elements to sample</typeparam>
            public class Cyclic<T>
            {
                private T[] source;
                private int index = 0;

                /// <summary>
                ///   Makes a cyclic samples using an input array.
                /// </summary>
                /// <param name="elements">The array of samples</param>
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

                /// <summary>
                ///   Gets a sequential element each time it is invoked.
                /// </summary>
                /// <returns>The element, which comes from the array</returns>
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
