namespace nu.Utility
{
    using System.Collections.Generic;

    public interface IArgumentMap
    {
        /// <summary>
        /// Applies the arguments to the specified object as they are enumerated
        /// </summary>
        /// <param name="obj">The object onto which the arguments should be applied</param>
        /// <param name="arguments">An enumerator of arguments being applied</param>
        void ApplyTo(object obj, IEnumerator<IArgument> arguments);
    }
}