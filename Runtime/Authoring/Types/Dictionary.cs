using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlephVault.Unity.Support.Generic
{
    namespace Authoring
    {
        namespace Types
        {
            /// <summary>
            ///   <para>
            ///     This is a regular dictionary but also provides means for its serialization and deserializaation into
            ///     the Inspector. To use them in the inspector, a subclass must be created (since the inspector does
            ///     not support generics) with a [System.Serializable] attribute tag, and also a subclass of
            ///     <see cref="DictionaryPropertyDrawer"/> must be created with an attribute tag like this:
            ///     [CustomPropertyDrawer(typeof(TheNewDictionarySubclass))]
            ///   </para>
            ///   <para>
            ///     This class was stolen, but I don't remember where did I take from.
            ///   </para>
            /// </summary>
            /// <typeparam name="TKey">The key. You may want to pay attention to its <c>GetHashCode()</c> method</typeparam>
            /// <typeparam name="TValue">The value. No considerations here</typeparam>
            public class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>, ISerializationCallbackReceiver
            {
                /// <summary>
                ///   The serialized keys.
                /// </summary>
                [SerializeField]
                TKey[] m_keys;

                /// <summary>
                ///   The serialized values.
                /// </summary>
                [SerializeField]
                TValue[] m_values;

                public Dictionary() : base() { }
                public Dictionary(IDictionary<TKey, TValue> dict) : base(dict) { }
                public Dictionary(IDictionary<TKey, TValue> dict, IEqualityComparer<TKey> comparer) : base(dict, comparer) { }
                public Dictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
                public Dictionary(Int32 capacity) : base(capacity) { }
                public Dictionary(Int32 capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

                public void CopyFrom(IDictionary<TKey, TValue> dict)
                {
                    this.Clear();
                    foreach (var kvp in dict)
                    {
                        this[kvp.Key] = kvp.Value;
                    }
                }

                public void OnAfterDeserialize()
                {
                    if (m_keys != null && m_values != null && m_keys.Length == m_values.Length)
                    {
                        this.Clear();
                        int n = m_keys.Length;
                        for (int i = 0; i < n; ++i)
                        {
                            this[m_keys[i]] = m_values[i];
                        }

                        m_keys = null;
                        m_values = null;
                    }

                }

                public void OnBeforeSerialize()
                {
                    int n = this.Count;
                    m_keys = new TKey[n];
                    m_values = new TValue[n];

                    int i = 0;
                    foreach (var kvp in this)
                    {
                        m_keys[i] = kvp.Key;
                        m_values[i] = kvp.Value;
                        ++i;
                    }
                }
            }
        }
    }
}
