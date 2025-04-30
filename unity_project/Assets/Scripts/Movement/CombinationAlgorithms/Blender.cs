using System.Collections.Generic;
using UnityEngine;

namespace Movement.CombinationAlgorithms
{
    /// <summary>
    /// This class represent the weighted blending algorithm.
    /// It returns the sum of the directions of the behaviours weighted by the weights.
    /// The combination is implemented in the GetDirection method, as if it was
    /// a movement behaviours.
    /// </summary>
    public class Blender : IMovement
    {
        private readonly List<IMovement> _behaviours = new();
        private readonly List<float> _weights = new();

        /// <summary>
        /// Add a behaviour with associated weight
        /// </summary>
        /// <param name="behaviour">
        /// The behaviour to add
        /// </param>
        /// <param name="weight">
        /// The weight of the behaviour. The higher the weight, the higher the importance
        /// </param>
        /// <returns>
        /// The blender itself
        /// </returns>
        public Blender Add(IMovement behaviour, float weight)
        {
            _behaviours.Add(behaviour);
            _weights.Add(weight);
            return this;
        }
        
        public Vector2 GetDirection()
        {
            var direction = Vector2.zero;
            for (var i = 0; i < _behaviours.Count; i++)
            {
                direction += _behaviours[i].GetDirection() * _weights[i];
            }
            return direction.normalized;
        }
    }
}