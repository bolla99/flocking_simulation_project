using UnityEngine;

namespace Movement
{
    /// <summary>
    /// This class represents the Arrive movement behaviour, which delegates
    /// To Seek if the target is far away, and stops if the target is close enough
    /// </summary>
    public class Arrive : Seek
    {
        private readonly float _targetRadius;
        /// <summary>
        /// </summary>
        /// <param name="boid">The agent</param>
        /// <param name="target">The target</param>
        /// <param name="targetRadius">The range within which the behaviour return the zero vector</param>
        public Arrive(
            IKinematic boid,
            IKinematic target,
            float targetRadius
        ) : base(boid, target)
        {
            _targetRadius = targetRadius;
        }

        public override Vector2 GetDirection()
        {
            var direction = _target.GetPosition() - _boid.GetPosition();
            var distance = direction.magnitude;

            if (distance < _targetRadius)
            {
                return Vector2.zero;
            }
            return base.GetDirection();
        }
    }
}