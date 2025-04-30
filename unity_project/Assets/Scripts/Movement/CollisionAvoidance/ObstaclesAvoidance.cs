using System.Linq;
using UnityEngine;
using Controllers;

namespace Movement.CollisionAvoidance
{
    /// <summary>
    /// This class represent the obstacle avoidance behaviour.
    /// It makes the boid avoid obstacles by predicting the time of closest approach
    /// and delegating to Flee with respect to the future position of the obstacle
    /// with the lowest time for closest approach and the future position of the boid at that time.
    /// </summary>
    public class ObstaclesAvoidance : NeighborhoodSensitiveMovement
    {
        private static readonly float Epsilon = 0.01f;
        private readonly float _collisionPredictionThreshold;

        /// <summary>
        /// Constructs a ObstaclesAvoidance behaviour.
        /// </summary>
        /// <param name="boid">
        /// The agent to which the behaviour is applied.
        /// It must implement IKinematic and INeighborhoodSensitive.
        /// </param>
        /// <param name="radius">
        /// The radius of the neighbourhood to consider for obstacles.
        /// </param>
        /// <param name="collisionPredictionThreshold">
        /// The range within which the boid and the obstacles are considered to collide.
        /// </param>
        public ObstaclesAvoidance(object boid, float radius, float collisionPredictionThreshold) : base(boid, radius)
        {
            _collisionPredictionThreshold = collisionPredictionThreshold;
        }

        public override Vector2 GetDirection()
        {
            // get the obstacles from the boid
            var obstacles = _boidNeighborhoodSensitive.GetNeighbors<ObstacleController>(_radius).ToArray();
            
            // there are no obstacles nearby
            if (!obstacles.Any()) return Vector2.zero;
            
            // get time for closest approach between for each obstacle
            // and select the first occurring closest approach
            var approaches = obstacles
                .Select(obstacle => (Obstacle: obstacle, Time: GetTimeForClosestApproach(_boidKinematic, obstacle)))
                .Where(closestApproach => closestApproach.Time >= 0f)
                .OrderBy(obstacle => obstacle.Time);
            
            foreach (var approach in approaches)
            {

                var timeForClosestApproach = approach.Time;
                var obstacle = approach.Obstacle;
                
                // TRAP MANAGEMENT
                if ((_boidKinematic.GetPosition() - obstacle.GetPosition()).magnitude <= 2f + Epsilon
                    && obstacle.GetVelocity().magnitude > _boidKinematic.GetVelocity().magnitude
                    && Vector2.Dot((_boidKinematic.GetPosition() - obstacle.GetPosition()).normalized,
                        obstacle.GetVelocity().normalized) > 0.5f)
                {
                    var perpendicularDirection = Vector2.Perpendicular(obstacle.GetVelocity()).normalized;
                    if (Vector2.Dot(perpendicularDirection,
                            (obstacle.GetPosition() - _boidKinematic.GetPosition()).normalized) > 0f)
                    {
                        perpendicularDirection = -perpendicularDirection;
                    }

                    return perpendicularDirection;
                }

                // future kinematics of boid and obstacle at the time of closest approach
                var boidFutureKinematic = FutureKinematic(_boidKinematic, timeForClosestApproach);
                var obstacleFutureKinematic = FutureKinematic(obstacle, timeForClosestApproach);

                // if the boid is not going to collide with the obstacle reuturn zero
                if ((boidFutureKinematic.GetPosition() - obstacleFutureKinematic.GetPosition()).magnitude >
                    _collisionPredictionThreshold)
                {
                    continue;
                }

                var futureFleeDirection = new Flee(boidFutureKinematic, obstacleFutureKinematic).GetDirection();
                
                // if the future flee direction they will cause collision, then return
                // the flee direction with the current kinematics
                if (WillCollide(new Kinematic(_boidKinematic.GetPosition(), 10f * futureFleeDirection), obstacle))
                    //&& obstacle.GetVelocity().magnitude > Mathf.Abs(futureFleeDirection.x) * _boidKinematic.GetVelocity().magnitude
                    //&& Vector2.Dot(futureFleeDirection, obstacle.GetVelocity()) > 0f)
                {
                    return new Flee(_boidKinematic, obstacle).GetDirection();
                }
                return futureFleeDirection;
            }
            return Vector2.zero;
        }
        
        private static float GetTimeForClosestApproach(IKinematic boid, IKinematic obstacle)
        {
            // check if already colliding
            if ((boid.GetPosition() - obstacle.GetPosition()).magnitude <= 2f + Epsilon)
            {
                return 0f;
            }
            var distanceVector = obstacle.GetPosition() - boid.GetPosition();
            var relativeVelocity = boid.GetVelocity() - obstacle.GetVelocity();
            var speedAlongDistanceVector = Vector2.Dot(relativeVelocity, distanceVector.normalized);
            return distanceVector.magnitude / speedAlongDistanceVector;
        }
        
        private bool WillCollide(IKinematic boid, IKinematic obstacle)
        {
            var closestApproachTime = GetTimeForClosestApproach(boid, obstacle);
            if (closestApproachTime < 0f) return false;
            var boidFutureKinematic = new Kinematic(boid.GetPosition() + boid.GetVelocity() * closestApproachTime);
            var obstacleFutureKinematic = new Kinematic(obstacle.GetPosition() + obstacle.GetVelocity() * closestApproachTime);
            return (boidFutureKinematic.GetPosition() - obstacleFutureKinematic.GetPosition()).magnitude <= 2f + Epsilon;
        }

        private Kinematic FutureKinematic(IKinematic kinematic, float time)
        {
            return new Kinematic(kinematic.GetPosition() + kinematic.GetVelocity() * time);
        }
    }
}