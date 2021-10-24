namespace AlephVault.Unity.Support.Generic
{
    namespace Types
    {
        /// <summary>
        ///   A contract to make the interfaces clonable
        ///   but in a type-aware way now. Beware: you
        ///   will have covariance issues if you, later
        ///   use inheritance. Clonable clases are only
        ///   useful if you don't plan to inherit it.
        /// </summary>
        /// <typeparam name="T">The cloned type - typically used in a T : <see cref="ICopy{T}"/> restriction</typeparam>
        public interface ICopy<T>
        {
            /// <summary>
            ///   Creates a copy of itself, either deeply
            ///   or shallowly.
            /// </summary>
            /// <param name="deep">Whether to do a deep copy or not - pure value types may ignore this argument at all</param>
            /// <returns>A copy of the object</returns>
            public T Copy(bool deep = false);
        }
    }
}