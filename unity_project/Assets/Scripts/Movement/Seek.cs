using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Seek movement behaviour that makes the agent move towards the target
    /// </summary>
    public class Seek : VariableMatching
    {
        public Seek(
            IKinematic boid,
            IKinematic target
            ) : base(boid, target) { }

        public override Vector2 GetDirection()
        {
            return (_target.GetPosition() - _boid.GetPosition()).normalized;
        }
    }
}