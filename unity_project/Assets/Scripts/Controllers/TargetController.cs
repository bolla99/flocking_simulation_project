using UnityEngine;
using Movement;

namespace Controllers
{
    /// <summary>
    /// This class represents the target that the boids will try to reach.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class TargetController : MonoBehaviour, IKinematic
    {
        private Camera _camera;
        private Rigidbody2D _rb;
        
        private void Awake()
        {
            _camera = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            SetRandomPosition();
        }
        
        private void Update()
        {
            if (GameManager.Instance().MouseSeekingModeEnabled)
            {
                MoveWithMouse(); 
            }
        }

        // if target mode is random, set random position when a boid collides with the target
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!GameManager.Instance().MouseSeekingModeEnabled && other.gameObject.layer == LayerMask.NameToLayer("Boid"))
            {
                SetRandomPosition();
            }
        }

        public Vector2 GetPosition()
        {
            return _rb.position;
        }

        public Vector2 GetVelocity()
        {
            return Vector2.zero;
        }

        private void MoveWithMouse()
        {
            var worldMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _rb.MovePosition(new Vector2(worldMousePosition.x, worldMousePosition.y));
        }
        
        /// <summary>
        /// Set the target position by choosing a corner (not the current one) and a random position
        /// within 10 units form the corner.
        /// </summary>
        public void SetRandomPosition()
        {
            while (true)
            {
                var angle = Random.Range(0, 360);
                var x = Mathf.Cos(Mathf.Rad2Deg * angle);
                var y = Mathf.Sin(Mathf.Deg2Rad * angle);
                var direction = new Vector2(x, y).normalized;
                var extent = Random.Range(0f, 10f);
                var basePosition = new Vector2(Mathf.Sign(x) * -50f, Mathf.Sign(y) * -50f);
                var relativePosition = extent * direction;
                if (Mathf.Abs(relativePosition.x) < 1f) relativePosition.x = Mathf.Sign(relativePosition.x) * 1f;
                if (Mathf.Abs(relativePosition.y) < 1f) relativePosition.y = Mathf.Sign(relativePosition.y) * 1f;
                var newPosition = basePosition + relativePosition;
                if (!(Vector2.Distance(_rb.position, newPosition) > 20f)) continue;
                _rb.position = newPosition;
                break;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(-50f, -50f, 0f), 10f);
            Gizmos.DrawWireSphere(new Vector3(50f, -50f, 0f), 10f);
            Gizmos.DrawWireSphere(new Vector3(-50f, 50f, 0f), 10f);
            Gizmos.DrawWireSphere(new Vector3(50f, 50f, 0f), 10f);
        }
    }
}
