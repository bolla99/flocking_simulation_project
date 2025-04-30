using Controllers;
using UnityEngine;
using PCG;

namespace Managers
{
    /// <summary>
    /// This class is responsible for managing a flock of boids.
    /// Place the FlockManager prefab in the scene to create a flock of boids.
    /// It places the boids in the scene with the Poisson Disk Sampling algorithm distributing
    /// them around the center of the flock, which is the position of the FlockManager.
    /// FlockManager overrides the boids prefab settings when instantiating them.
    /// Since FlockManager instantiates the boids as a flock,
    /// it is responsible for setting the boids detection radius.
    /// </summary>
    public class FlockManager : MonoBehaviour
    {
        public Settings Settings;
        public float BoidRadius = 1f;
        public GameObject BoidPrefab;
        
        [Header("Boid Prefab Overrides")]
        public GameObject Target;
        public int MaxNeighbors = 100;

        // Start is called before the first frame update
        private void Start()
        {
            SetBoidsDetectionRadius();
            PlaceBoids(); 
        }
        
        private void PlaceBoids()
        {
            var positions = PoissonDiskSampling.GeneratePoints(
                BoidRadius,
                new Vector2(100f, 100f),
                (Vector2)transform.position + new Vector2(50f, 50f),
                Settings.BoidsCount
            );
            for (var i = 0; i < positions.Count; i++)
            {
                var boid = Instantiate(BoidPrefab, positions[i] + new Vector2(-50f, -50f), Quaternion.identity);
                OverrideBoid(boid.GetComponent<BoidController>());
                boid.transform.parent = transform;
                boid.name = $"Boid_{i}";
            }
        }

        private void OverrideBoid(BoidController boid) 
        {
            boid.Target = Target;
            boid.MaxNeighbors = MaxNeighbors;
        }
        
        private void SetBoidsDetectionRadius()
        {
            GameManager.Instance().BoidsDetectionRadius = Mathf.Max(7f, 2f * Mathf.Sqrt(Settings.BoidsCount));
        }
    }
}
