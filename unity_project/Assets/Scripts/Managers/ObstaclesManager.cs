using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Controllers;

namespace Managers
{
    /// <summary>
    /// This class is responsible for managing the obstacles in the scene.
    /// Place a GameObject with ObstaclesManager script in the scene to
    /// generate the obstacles.
    /// ObstaclesManager places the obstacles randomly in the scene
    /// and makes sure they do not overlap.
    /// </summary>
    public class ObstaclesManager : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Settings scriptable object
        /// </summary>
        public Settings Settings;
        
        /// <summary>
        ///  The radius of the circle that represents an obstacle.
        /// </summary>
        public float ObstacleRadius;
        /// <summary>
        /// The minimum position where the obstacles can be placed.
        /// </summary>
        public Vector2 MinPosition = new Vector2(-50f, -50f);
        /// <summary>
        /// The maximum position where the obstacles can be placed.
        /// </summary>
        public Vector2 MaxPosition = new Vector2(50f, 50f);
        /// <summary>
        /// The obstacle prefab to instantiate.
        /// </summary>
        public GameObject ObstaclePrefab;

        private IList<ObstacleController> _obstacles;

        // Start is called before the first frame update
        private void Start()
        {
            _obstacles = new List<ObstacleController>();
            PlaceObstacles();
        }

        // Update is called once per frame

        private void PlaceObstacles()
        {
            var radiusCorrectedMinPosition =
                new Vector2(MinPosition.x + ObstacleRadius, MinPosition.y + ObstacleRadius);
            var radiusCorrectedMaxPosition =
                new Vector2(MaxPosition.x - ObstacleRadius, MaxPosition.y - ObstacleRadius);

            var obstaclesCount = Settings.ObstaclesCount;
            for (var i = 0; i < obstaclesCount; i++)
            {
                Vector2 position;
                do
                {
                    position = new Vector2(
                        Random.Range(radiusCorrectedMinPosition.x, radiusCorrectedMaxPosition.x),
                        Random.Range(radiusCorrectedMinPosition.y, radiusCorrectedMaxPosition.y)
                    );
                } while (!IsObstaclePlacementValid(position));

                PlaceObstacle(position, i);
            }
        }

        private void PlaceObstacle(Vector2 position, int i)
        {
            var obstacle = Instantiate(ObstaclePrefab, position, Quaternion.identity);
            _obstacles.Add(obstacle.GetComponent<ObstacleController>());
            obstacle.transform.parent = transform;
            obstacle.name = $"Obstacle_{i}";
        }

        private bool IsObstaclePlacementValid(Vector2 position)
        {
            return _obstacles.All(obstacle => Mathf.Abs(position.y - obstacle.transform.position.y) > ObstacleRadius * 2f);
        }
    }
}
