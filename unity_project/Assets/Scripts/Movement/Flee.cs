using UnityEngine;

namespace Movement
{
    /// <summary>
    /// This class represents the Flee movement behaviour that makes the agent move away from the target
    /// </summary>
    public class Flee : VariableMatching
    {
        public Flee(
            IKinematic boid,
            IKinematic target
            ) : base(boid, target) { }

        public override Vector2 GetDirection()
        {
            return (_boid.GetPosition() - _target.GetPosition()).normalized;
        }
    }
}