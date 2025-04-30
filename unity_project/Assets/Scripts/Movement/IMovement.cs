using UnityEngine;

namespace Movement
{
    /// <summary>
    /// This interface represent the ability to return a direction;
    /// every movement behaviour must implement this interface.
    /// </summary>
    public interface IMovement
    {
        /// <summary>
        /// Returns the direction the object should move in
        /// </summary>
        /// <returns>
        /// The returned vector can be a normalized vector or the zero vector
        /// </returns>
        public Vector2 GetDirection();
    }
}