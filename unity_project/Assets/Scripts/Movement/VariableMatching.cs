using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Abstract class that represent a movement behaviour
    /// that is based on the difference between two kinematics,
    /// the agent and a target kinematic.
    /// </summary>
    public abstract class VariableMatching : IMovement
    {
        /// <summary>
        /// The agent that will be moved by this movement behaviour
        /// </summary>
        protected IKinematic _boid;
        /// <summary>
        /// The target that the agent will try to match
        /// </summary>
        protected IKinematic _target;
        
        protected VariableMatching(IKinematic boid, IKinematic target)
        {
            _boid = boid;
            _target = target;
        }
        
        public abstract Vector2 GetDirection();
    }
}