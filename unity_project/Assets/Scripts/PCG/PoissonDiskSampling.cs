using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    /// <summary>
    /// This class provides a method to generate samples in a 2D space
    /// using the Poisson Disk Sampling algorithm.
    /// </summary>
    public static class PoissonDiskSampling
    {
        /// <summary>
        /// This method generates a list of samples in a 2D space
        /// using the Poisson Disk Sampling algorithm.
        /// The distribution of the samples is centered around the CenterOfDistribution,
        /// And because the region is meant to be bigger than the area
        /// occupied by the samples, the samples are generated from the
        /// centre of distribution and then spread outwards.
        /// </summary>
        /// <param name="radius">
        /// Half of the minimum distance between samples
        /// </param>
        /// <param name="sampleRegionSize">
        /// Size of the region where the samples are generated
        /// </param>
        /// <param name="centreOfDistribution">
        /// The point around which the samples are distributed
        /// </param>
        /// <param name="samplesCount">
        /// The number of samples to generate
        /// </param>
        /// <param name="samplesBeforeRejectionCount">
        /// Times to try to generate a sample before giving up
        /// </param>
        /// <returns>
        /// A list of the samples generated
        /// </returns>
        public static List<Vector2> GeneratePoints(
            float radius,
            Vector2 sampleRegionSize,
            Vector2 centreOfDistribution,
            int samplesCount,
            int samplesBeforeRejectionCount = 30
        )
        {
            var cellSize = radius / Mathf.Sqrt(2);
            var gridSize = new Vector2Int(
                Mathf.CeilToInt(sampleRegionSize.x / cellSize),
                Mathf.CeilToInt(sampleRegionSize.y / cellSize)
            );
            var grid = new int[gridSize.x, gridSize.y];
            InitGrid(grid);
            var points = new List<Vector2>();
            var spawnPoints = new List<Vector2> { centreOfDistribution };
            while (spawnPoints.Count > 0 && points.Count < samplesCount)
            {
                var spawnPoint = spawnPoints[0];
                var candidateAccepted = false;
                for (var i = 0; i < samplesBeforeRejectionCount; i++)
                {
                    var angle = Random.value * Mathf.PI * 2;
                    var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
                    var candidate = spawnPoint + direction * Random.Range(radius, 2f * radius);
                    if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                    {
                        grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                        points.Add(candidate);
                        spawnPoints.Add(candidate);
                        candidateAccepted = true;
                        break;
                    }
                }

                if (!candidateAccepted)
                {
                    spawnPoints.RemoveAt(0);
                }
            }

            return points;
        }

        private static bool IsValid(
            Vector2 candidate,
            Vector2 regionSize,
            float cellSize,
            float radius,
            List<Vector2> points,
            int[,] grid
        )
        {

            var candidateX = (int)(candidate.x / cellSize);
            var candidateY = (int)(candidate.y / cellSize);

            var minX = Mathf.Max(0, candidateX - 3);
            var minY = Mathf.Max(0, candidateY - 3);
            var maxX = Mathf.Min(grid.GetLength(0) - 1, candidateX + 3);
            var maxY = Mathf.Min(grid.GetLength(1) - 1, candidateY + 3);

            for (var i = minX; i <= maxX; i++)
            {
                for (var j = minY; j <= maxY; j++)
                {
                    var pointIndex = grid[i, j];
                    if (pointIndex >= 0)
                    {
                        var sqrDistance = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDistance < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static void InitGrid(int[,] grid)
        {
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = -1;
                }
            }
        }
    }
}