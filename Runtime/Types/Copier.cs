using System;
using System.Collections.Generic;


namespace AlephVault.Unity.Support.Generic
{
    namespace Types
    {
        /// <summary>
        ///   Provides a way to copy an object of the required
        ///   type (be it a reference or value type), allowing
        ///   a shallow or deep copy of it.
        /// </summary>
        /// <typeparam name="T">The cloned type - typically used in a T : <see cref="ICopy{T}"/> restriction</typeparam>
        public abstract class Copier<T>
        {
            /// <summary>
            ///   Copies the given object, either shallowly or deeply.
            ///   Performs a typecheck to require the exact type.
            /// </summary>
            /// <param name="source">The object to copy</param>
            /// <param name="deep">Whether to do a deep copy or not - pure value types may ignore this argument at all</param>
            /// <returns>A copy of the object</returns>
            public T Copy(T source, bool deep = false)
            {
                if (EqualityComparer<T>.Default.Equals(source, default(T))) return default(T);

                if (source.GetType() != typeof(T)) throw new ArgumentException($"Copied source is not exactly of type {typeof(T).FullName}");

                return DoCopy(source, deep);
            }

            /// <summary>
            ///   This method must be overridden to actually copy the object.
            ///   At this point, the value is != default(T), and is of exactly
            ///   type T, and not a descendant of it.
            /// </summary>
            /// <param name="source">The object to copy</param>
            /// <param name="deep">Whether to do a deep copy or not - pure value types may ignore this argument at all</param>
            /// <returns>A copy of the object</returns>
            protected abstract T DoCopy(T source, bool deep);
        }
    }
}