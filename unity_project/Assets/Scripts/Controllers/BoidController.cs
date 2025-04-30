using System.Collections.Generic;
using System.Linq;
using Movement;
using Unity.VisualScripting;
using UnityEngine;
using Rigidbody2D = UnityEngine.Rigidbody2D;
using Movement.FlockingAlgorithms;
using Movement.CombinationAlgorithms;
using Movement.CollisionAvoidance;

namespace Controllers
{
    /// <summary>
    /// This class represent a boid.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BoidController : 
        MonoBehaviour, 
        IKinematic, 
        INeighborhoodSensitive
    {
        private Rigidbody2D _rb;
        private Vector2 _direction;
        private Vector2 _targetDirection;
        private Rigidbody2D _target;
        private IMovement _movementBehaviour;
        private Collider2D[] _neighborhood;
        private int _neighborhoodCount;

        private float _updateNeighborhoodRadius;

        /// <summary>
        /// Reference to the Settings scriptable object.
        /// </summary>
        public Settings Settings;
        /// <summary>
        /// The target that the boid will try to reach.
        /// </summary>
        public GameObject Target; 
        /// <summary>
        /// Max number of neighbours that can be detected by the boid at once.
        /// </summary>
        public int MaxNeighbors = 100;
        
        private void Awake()
        {
            _neighborhood = new Collider2D[MaxNeighbors];
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SetUpMovementBehaviour();
            _updateNeighborhoodRadius = Mathf.Max(Settings.ObstaclesDetectionRadius, GameManager.Instance().BoidsDetectionRadius);
        }

        private void Update()
        {
            _targetDirection = _movementBehaviour.GetDirection();
            _direction = Vector2.Lerp(
                _direction, _targetDirection, Time.deltaTime * Settings.SmoothingMultiplier
                );
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + Settings.BoidsSpeed * Time.fixedDeltaTime * _direction);
            UpdateNeighborhood();
        }

        private void UpdateNeighborhood()
        {
            _neighborhoodCount = Physics2D.OverlapCircleNonAlloc(_rb.position, _updateNeighborhoodRadius, _neighborhood);
        }
        
        public IEnumerable<T> GetNeighbors<T>(float radius)
        {
            return _neighborhood
                .Take(_neighborhoodCount)
                .Where(c => Vector2.Distance(transform.position, c.gameObject.transform.position) <= radius)
                .Select(c => c.GetComponent<T>())
                .NotNull();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(GetPosition(), _direction * 10f);
            Gizmos.DrawWireSphere(GetPosition(), GameManager.Instance().BoidsDetectionRadius);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(GetPosition(), _targetDirection * 10f);
            
        }
        
        public Vector2 GetPosition()
        {
            return _rb ? _rb.position : Vector2.zero;
        }
        
        public Vector2 GetVelocity()
        {
            var velocity = Settings.BoidsSpeed * _direction;
            if (_direction.magnitude < 0.5) velocity = Settings.BoidsSpeed * _targetDirection;
            return velocity;
        }

        private void SetUpMovementBehaviour()
        {
            var arrive = new Arrive(
                this, 
                Target.GetComponent<IKinematic>(), 
                1f
            );
            var obstaclesAvoidance = new ObstaclesAvoidance(this, Settings.ObstaclesDetectionRadius, Settings.CollisionPredictionThreshold);
            var wallAvoidance = new WallAvoidance(this, 2f);
            var collisionAvoidanceBlender = new Blender()
                .Add(obstaclesAvoidance, 1f)
                .Add(wallAvoidance, 1f);
            
            var boidsDetectionRadius = GameManager.Instance().BoidsDetectionRadius;
            var separation = new Separate(this, boidsDetectionRadius);
            var cohesion = new Cohesion(this, boidsDetectionRadius);
            var alignment = new Alignment(this, boidsDetectionRadius);

            var flockingBlender = new Blender()
                .Add(separation, Settings.SeparationWeight)
                .Add(cohesion, Settings.CohesionWeight)
                .Add(alignment, Settings.AlignmentWeight)
                .Add(arrive, 1.2f)
                .Add(wallAvoidance, 3f);
            
            _movementBehaviour = new PriorityBlending()
                .Add(collisionAvoidanceBlender, 1f)
                .Add(flockingBlender, 2f);
        }
    }
}
