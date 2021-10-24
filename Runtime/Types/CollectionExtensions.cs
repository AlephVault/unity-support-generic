using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AlephVault.Unity.Support.Generic
{
    namespace Types
    {
        public static class CollectionExtensions
        {
            /// <summary>
            ///   A shortcut to set a default value (and return it)
            ///   into a dictionary if no value exists for a given
            ///   key (and just returning it, if it is present).
            /// </summary>
            /// <typeparam name="K">The key type</typeparam>
            /// <typeparam name="V">The value type</typeparam>
            /// <param name="dict">The dictionary</param>
            /// <param name="key">The key</param>
            /// <param name="default">The value to store, if the key is absent</param>
            /// <returns>The new default, or current, value</returns>
            public static V SetDefault<K, V>(this IDictionary<K, V> dict, K key, V @default)
            {
                V value;
                if (!dict.TryGetValue(key, out value))
                {
                    dict.Add(key, @default);
                    return @default;
                }
                else
                {
                    return value;
                }
            }
        }
    }
}