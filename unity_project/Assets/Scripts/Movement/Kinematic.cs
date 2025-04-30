using UnityEngine;

namespace Movement
{
    /// <summary>
    /// This class implements IKinematic and represents a kinematic object;
    /// although boids and obstacles are represented by other classes that implements
    /// IKinematic, this bare-bone IKinematic implementation is meant to store
    /// temporary Kinematics created at runtime, such as in the ObstaclesAvoidance
    /// GetDirection implementation.
    /// </summary>
    public class Kinematic : IKinematic
    {
        private readonly Vector2 _position;
        private readonly Vector2 _velocity;
        
        public Kinematic(Vector2 position)
        {
            _position = position;
            _velocity = Vector2.zero;
        }
        
        public Kinematic(Vector2 position, Vector2 velocity)
        {
            _position = position;
            _velocity = velocity;
        }
        public Vector2 GetPosition()
        {
            return _position;
        }

        public Vector2 GetVelocity()
        {
            return _velocity;
        }
    }
}