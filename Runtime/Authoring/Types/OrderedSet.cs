using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlephVault.Unity.Support.Generic
{
    namespace Authoring.Types
    {
        /// <summary>
        ///   <para>
        ///     This is a set (i.e. it keeps unique elements) that considers the order of the elements
        ///     as they are being added as well,vv for iteration. When deserialized, elements' positions
        ///     are tracked as they appear (duplicates are discarded without tracking), and also they
        ///     are respected on deserialization. To use them in the inspector, a subclass must be
        ///     created (since the inspector does not support generics) with a [System.Serializable]
        ///     attribute tag, and also a subclass of <see cref="OrderedSetPropertyDrawer"/> must be
        ///     created with an attribute tag like this:
        ///     [CustomPropertyDrawer(typeof(TheNewOrderedSetSubclass))]
        ///   </para>
        ///   <para>
        ///     This class was stolen, but I don't remember where did I take from.
        ///   </para>
        /// </summary>
        /// <typeparam name="T">The value. You may want to pay attention to its <c>GetHashCode()</c> method</typeparam>
        public class OrderedSet<T> : ICollection<T>, ISerializationCallbackReceiver
        {
            /// <summary>
            ///   The serialized elements, considering its order.
            /// </summary>
            [SerializeField]
            T[] m_values;

            private readonly IDictionary<T, LinkedListNode<T>> m_Dictionary;
            private readonly LinkedList<T> m_LinkedList;

            public OrderedSet() : this(EqualityComparer<T>.Default) {}

            public OrderedSet(IEqualityComparer<T> comparer)
            {
                m_Dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
                m_LinkedList = new LinkedList<T>();
            }

            public int Count
            {
                get { return m_Dictionary.Count; }
            }

            public virtual bool IsReadOnly
            {
                get { return m_Dictionary.IsReadOnly; }
            }

            void ICollection<T>.Add(T item)
            {
                Add(item);
            }

            public void Clear()
            {
                m_LinkedList.Clear();
                m_Dictionary.Clear();
            }

            public bool Remove(T item)
            {
                LinkedListNode<T> node;
                bool found = m_Dictionary.TryGetValue(item, out node);
                if (!found) return false;
                m_Dictionary.Remove(item);
                m_LinkedList.Remove(node);
                return true;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return m_LinkedList.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public bool Contains(T item)
            {
                return m_Dictionary.ContainsKey(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                m_LinkedList.CopyTo(array, arrayIndex);
            }

            public bool Add(T item)
            {
                if (m_Dictionary.ContainsKey(item)) return false;
                LinkedListNode<T> node = m_LinkedList.AddLast(item);
                m_Dictionary.Add(item, node);
                return true;
            }

            public T Shift()
            {
                T element = First;
                m_LinkedList.RemoveFirst();
                m_Dictionary.Remove(element);
                return element;
            }

            public T Pop()
            {
                T element = Last;
                m_LinkedList.RemoveLast();
                m_Dictionary.Remove(element);
                return element;
            }

            public void OnBeforeSerialize()
            {
                m_values = new T[Count];
                CopyTo(m_values, 0);
            }

            public void OnAfterDeserialize()
            {
                if (m_values != null)
                {
                    CopyFrom(m_values);
                }
            }

            public void CopyFrom(IEnumerable<T> items)
            {
                Clear();
                foreach (var item in items)
                {
                    Add(item);
                }
            }

            public T Last
            {
                get
                {
                    return m_LinkedList.Last.Value;
                }
            }

            public T First
            {
                get
                {
                    return m_LinkedList.First.Value;
                }
            }
        }
    }
}
