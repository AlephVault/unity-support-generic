﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlephVault.Unity.Support.Generic
{
    namespace Authoring
    {
        namespace Types
        {
            public class InterfacedList<TContainer, TInterface> : IList<TInterface>
                where TInterface : class
                where TContainer : Interfaced<TInterface>, new()
            {
                private readonly Func<IList<TContainer>> _getList;

                public InterfacedList(Func<IList<TContainer>> getList)
                {
                    _getList = getList;
                }

                public int Count => _getList().Count;

                public bool IsReadOnly => _getList().IsReadOnly;

                public IEnumerator<TInterface> GetEnumerator()
                {
                    return _getList().Select(c => c?.Result).GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public void Add(TInterface item)
                {
                    _getList().Add(new TContainer { Result = item });
                }

                public void Clear()
                {
                    _getList().Clear();
                }

                public bool Contains(TInterface item)
                {
                    return IndexOf(_getList(), item) >= 0;
                }

                public void CopyTo(TInterface[] array, int arrayIndex)
                {
                    var list = _getList().Select(c => c?.Result).ToList();
                    Array.Copy(list.ToArray(), 0, array, arrayIndex, list.Count);
                }

                public bool Remove(TInterface item)
                {
                    var list = _getList();
                    var indexToRemove = IndexOf(list, item);
                    if (indexToRemove < 0)
                    {
                        return false;
                    }

                    list.RemoveAt(indexToRemove);
                    return true;
                }

                public int IndexOf(TInterface item)
                {
                    return IndexOf(_getList(), item);
                }

                public void Insert(int index, TInterface item)
                {
                    _getList().Insert(index, new TContainer { Result = item });
                }

                public void RemoveAt(int index)
                {
                    _getList().RemoveAt(index);
                }

                public TInterface this[int index]
                {
                    get => _getList()[index]?.Result;
                    set => _getList()[index] = new TContainer { Result = value };
                }

                private static int IndexOf(IList<TContainer> list, TInterface item)
                {
                    for (int i = 0; i < list.Count; ++i)
                    {
                        // The item might be null or assigned.
                        // If the list-item is null or its result
                        // is null, it will only match when the
                        // item becomes null itself.
                        if (list[i]?.Result == item) return i;
                    }

                    return -1;
                }
            }
        }
    }
}
