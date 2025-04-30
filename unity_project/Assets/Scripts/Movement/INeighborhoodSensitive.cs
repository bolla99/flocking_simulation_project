using System.Collections.Generic;

namespace Movement
{
    /// <summary>
    /// This interface represent the ability to return data about
    /// the neighbourhood. The neighbourhood is defined as a collection.
    /// </summary>
    public interface INeighborhoodSensitive
    {
        /// <summary>
        /// This method returns a neighborhood of objects of type T
        /// </summary>
        /// <typeparam name="T">
        /// The type of the objects in the neighborhood
        /// </typeparam>
        /// <param name="radius">
        /// The range within the neighbors are detected
        /// </param>
        /// <returns>
        /// it returns a collection of elements that are guaranteed to be
        /// not null
        /// </returns>
        IEnumerable<T> GetNeighbors<T>(float radius);
    }
}