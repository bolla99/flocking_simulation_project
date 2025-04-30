using System.Linq;
using UnityEngine;
using Controllers;

namespace Movement.FlockingAlgorithms
{
    /// <summary>
    /// This class represents the Separate movement behaviour that makes the agent move away from its neighbors;
    /// It delegates to Flee for each neighbour, blending the results together
    /// weighted by the inverse of the distance to the neighbour
    /// </summary>
    public class Separate : NeighborhoodSensitiveMovement
    {
        /// <summary>
        /// Constructs a Separate behaviour.
        /// </summary>
        /// <param name="boid">
        /// The agent that will be moved by this movement behaviour.
        /// It must implement the IKinematic and INeighborhoodSensitive
        /// </param>
        /// <param name="radius">
        /// /// The range within which the other boids are considered neighbours.
        /// </param>
        public Separate (object boid, float radius) : base(boid, radius) { }

        public override Vector2 GetDirection()
        {
            var neighbors = _boidNeighborhoodSensitive
                .GetNeighbors<BoidController>(_radius)
                .Where(boid => boid != _boidKinematic as BoidController)
                .ToArray();
            if (neighbors.Length == 0)
            {
                return Vector2.zero;
            }

            var direction = Vector2.zero;
            foreach (var neighbor in neighbors)
            {
                var fleeFromNeighborDir = _boidKinematic.GetPosition() - neighbor.GetPosition();
                direction += fleeFromNeighborDir / fleeFromNeighborDir.sqrMagnitude;
            }
            return direction.normalized;
        }
    }
}