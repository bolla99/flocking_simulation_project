using UnityEngine;

namespace Movement
{
    /// <summary>
    /// This abstract class represent a movement behaviour that needs
    /// kinematic data about the neighbourhood of the agent. The agent
    /// is required to implement the INeighborhoodSensitive interface
    /// and the IKinematic interface; to enforce this requirement the agent is
    /// referenced by two different fields, one for each interface.
    /// </summary>
    public abstract class NeighborhoodSensitiveMovement : IMovement
    {
        protected INeighborhoodSensitive _boidNeighborhoodSensitive;
        protected IKinematic _boidKinematic;
        protected float _radius;
        
        protected NeighborhoodSensitiveMovement(object boid, float radius)
        {
            if (boid is INeighborhoodSensitive boidNeighborhoodSensitive
                && boid is IKinematic boidKinematic)
            {
                _boidNeighborhoodSensitive = boidNeighborhoodSensitive;
                _boidKinematic = boidKinematic;
            }
            else
            {
                throw new System.ArgumentException("Boid must implement INeighborhoodSensitive and IKinematic interfacse");
            }
            _radius = radius;
        }
        public abstract Vector2 GetDirection();
    }
}