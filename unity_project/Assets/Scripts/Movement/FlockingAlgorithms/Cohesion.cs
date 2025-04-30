using System.Linq;
using UnityEngine;
using Controllers;

namespace Movement.FlockingAlgorithms
{
    /// <summary>
    /// This class represents the cohesion behaviour of a boid;
    /// it delegates to seek towards the average position of the neighbours.
    /// </summary>
    public class Cohesion : NeighborhoodSensitiveMovement
    {
        /// <summary>
        /// Constructs a Cohesion behaviour.
        /// </summary>
        /// <param name="boid">
        /// The agent that will be moved by this movement behaviour.
        /// It must implement the IKinematic and INeighborhoodSensitive
        /// </param>
        /// <param name="radius">
        /// /// The range within which the other boids are considered neighbours.
        /// </param>
        public Cohesion(object boid, float radius) : base(boid, radius) { }
        
        public override Vector2 GetDirection()
        {
            var neighbors = _boidNeighborhoodSensitive
                .GetNeighbors<BoidController>(_radius)
                .ToArray();
            if (neighbors.Length == 0)
            {
                return Vector2.zero;
            }
            var averagePosition = Vector2.zero;
            foreach (var neighbor in neighbors)
            {
                averagePosition += neighbor.GetPosition();
            }
            averagePosition /= neighbors.Length;

            return new Seek(
                _boidKinematic,
                new Kinematic(averagePosition)
            ).GetDirection();
        }
    }
}