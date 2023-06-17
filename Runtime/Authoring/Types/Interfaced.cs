using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AlephVault.Unity.Support.Generic
{
    namespace Authoring
    {
        namespace Types
        {
            namespace InterfacedCore
            {
                /// <summary>
                ///   This class serves as a parent to be used in custom property drawers
                ///   and related classes to draw these objects properly in editor time.
                ///   Users should instead derive new classes from <see cref="InterfacedCore"/>
                ///   and add the proper annotations as needed.
                /// </summary>
                [Serializable]
                public abstract class Parent
                {
                    [SerializeField]
                    [HideInInspector]
                    protected Object ObjectField;

                    //Used internally to display properly in drawer.
#pragma warning disable 414
                    [SerializeField]
                    [HideInInspector]
                    protected string ResultType;
#pragma warning restore 414

                    private static readonly Regex TypeArgumentsReplace = new Regex(@"`[0-9]+");
                    public static string FullConcreteTypeName(Type type)
                    {
                        var typeName = type.Name;

                        if (!type.IsGenericType)
                        {
                            return typeName;
                        }

                        var argumentsString = type.GetGenericArguments().Aggregate((string)null, (s, t) => s == null ? (FullConcreteTypeName(t)) : $"{s}, {(FullConcreteTypeName(t))}");
                        return TypeArgumentsReplace.Replace(typeName, $"<{argumentsString}>");
                    }
                }
            }

            /// <summary>
            ///   Defines an interfaced class. It accepts a <see cref="Result"/> type
            ///   which will typically be an interface, but can be any type. This,
            ///   provided that the assigned instances descend from <see cref="UnityEngine.Object"/>,
            ///   since this guarantees the serializer will always me able to store the reference.
            /// </summary>
            [Serializable]
            public abstract class Interfaced<TResult> : InterfacedCore.Parent
                where TResult : class
            {
                public TResult Result
                {
                    //Using the null coalescing operator will break web player execution
                    get
                    {
#if UNITY_EDITOR
                        if (ObjectField == null && string.IsNullOrEmpty(ResultType))
                        {
                            return _result = null;
                        }
                        if (string.IsNullOrEmpty(ResultType))
                        {
                            _result = null;
                        }
#endif

                        return _result ??= ObjectField as TResult;
                    }
                    set
                    {
                        _result = value;
                        ObjectField = _result as Object;

#if UNITY_EDITOR
                        if (!Application.isPlaying)
                        {
                            if (_result != null && ObjectField == null)
                            {
                                Debug.LogWarning($"{nameof(Interfaced<TResult>)}: The assigned object MUST be an instance of UnityEngine.Object.");
                                _result = null;
                            }
                        }
                        ResultType = _result != null ? FullConcreteTypeName(_result.GetType()) : "";
#endif
                    }
                }

                public Object Object =>
                    //Using the null coalescing operator will break web player execution
                    ObjectField != null ? ObjectField : (ObjectField = _result as Object);

                private TResult _result;
            }
        }
    }
}
