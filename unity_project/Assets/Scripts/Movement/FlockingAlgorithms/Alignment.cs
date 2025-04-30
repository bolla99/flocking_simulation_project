using System.Linq;
using UnityEngine;
using Controllers;

namespace Movement.FlockingAlgorithms
{
    /// <summary>
    /// this class represent the alignment behaviour;
    /// it returns the average velocity of the neighbours.
    /// </summary>
    public class Alignment : NeighborhoodSensitiveMovement
    {
        /// <summary>
        /// Constructs an Alignment behaviour.
        /// </summary>
        /// <param name="boid">
        /// The agent that will be moved by this movement behaviour.
        /// It must implement the IKinematic and INeighborhoodSensitive
        /// </param>
        /// <param name="radius">
        /// The range within which the other boids are considered neighbours.
        /// </param>
        public Alignment(object boid, float radius) : base(boid, radius) { }
        
        public override Vector2 GetDirection()
        {
            var neighbors = _boidNeighborhoodSensitive
                .GetNeighbors<BoidController>(_radius)
                .ToArray();
            if (neighbors.Length == 0)
            {
                return Vector2.zero;
            }
            var averageVelocity = Vector2.zero;
            foreach (var neighbor in neighbors)
            {
                averageVelocity += neighbor.GetVelocity();
            }
            averageVelocity /= neighbors.Length;

            return averageVelocity.normalized;
        }
    }
}