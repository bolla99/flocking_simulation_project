using UnityEngine;

namespace Movement
{
    /// <summary>
    /// This interface represent the ability to return the position and velocity,
    /// which are the kinematic data required by a movement behaviour.
    /// </summary>
    public interface IKinematic
    {
        /// <summary>
        /// Return the current position
        /// </summary>
        /// <returns>
        /// The position as a Vector2
        /// </returns>
        public Vector2 GetPosition();
        /// <summary>
        /// Return the current velocity
        /// </summary>
        /// <returns>
        /// The velocity as a Vector2
        /// </returns>
        public Vector2 GetVelocity();
    }
}