using UnityEngine;
using Random = UnityEngine.Random;
using Movement;

namespace Controllers
{
    /// <summary>
    /// This class represent an obstacle that moves back and forth at fixed speed.
    /// </summary>
    public class ObstacleController : MonoBehaviour, IKinematic
    {
        /// <summary>
        /// Reference to the Settings scriptable object.
        /// </summary>
        public Settings Settings;
        
        private int _direction = 0;
        private float _speed;
        private Rigidbody2D _rb;

        private void Start()
        {
            _direction = Random.Range(0, 1);
            _speed = Random.Range(Settings.ObstaclesMinSpeed, Settings.ObstaclesMaxSpeed);

            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 movement = _direction == 0 ? Vector2.right : Vector2.left;
            _rb.MovePosition(_rb.position + _speed * Time.fixedDeltaTime * movement);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                _direction = 1 - _direction;
            }
        }

        public Vector2 GetPosition()
        {
            return _rb.position;
        }

        public Vector2 GetVelocity()
        {
            return _speed * (_direction == 0 ? Vector2.right : Vector2.left);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(GetPosition(), GetVelocity());
        }
    }
}