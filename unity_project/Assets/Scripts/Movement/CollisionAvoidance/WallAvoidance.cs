using UnityEngine;

namespace Movement.CollisionAvoidance
{
    /// <summary>
    /// This class represents the WallAvoidance movement behaviour
    /// that makes the agent avoid walls.
    /// The behaviour does not simply avoid walls, but it avoids scratching
    /// walls by moving parallel to them.
    /// </summary>
    public class WallAvoidance : IMovement
    {
        private readonly IKinematic _boid;
        private readonly float _reach;

        /// <summary>
        /// Constructs a WallAvoidance instance by specifying the agend and the reach
        /// </summary>
        /// <param name="boid">
        /// The agent that will be moved by this movement behaviour
        /// </param>
        /// <param name="reach">
        /// The length of the ray that will be casted to detect walls
        /// </param>
        public WallAvoidance(IKinematic boid, float reach)
        {
            _boid = boid;
            _reach = reach;
        }
        
        public Vector2 GetDirection()
        {
            Vector2 direction = Vector2.zero;
            var velocityXComponent = new Vector2(_boid.GetVelocity().x, 0f);
            var velocityYComponent = new Vector2(0f, _boid.GetVelocity().y);
            
            // check the component with the smallest magnitude
            var componentToCheck = velocityXComponent.magnitude < velocityYComponent.magnitude ? velocityXComponent : velocityYComponent;
            
            var hit = Physics2D.Raycast(_boid.GetPosition(), componentToCheck, _reach, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                //direction = (hit.point + 3f * hit.normal) - _boid.GetPosition();
                direction = hit.normal;
            }
            
            return direction.normalized;
        }
    }
}