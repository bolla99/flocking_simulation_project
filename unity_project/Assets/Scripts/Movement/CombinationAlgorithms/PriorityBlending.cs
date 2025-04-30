using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Movement.CombinationAlgorithms
{
    /// <summary>
    /// This class represent the Priority Blending algorithm.
    /// It holds a list of movements (usually weighted blenders)
    /// with an associated priority.
    /// It returns the direction of the lowest priority movement that
    /// returns a non-zero direction.
    /// The combination is implemented in the GetDirection method, as if it was
    /// a movement behaviour.
    /// </summary>
    public class PriorityBlending : IMovement
    {
        private List<(IMovement Movement, float Priority)> _blenders = new();

        /// <summary>
        /// Add a movement with associated priority
        /// </summary>
        /// <param name="movement">
        /// Movement to add
        /// </param>
        /// <param name="priority">
        /// Priority of the movement. The lower the priority, the higher the importance
        /// </param>
        /// <returns>
        /// The priority blender itself.
        /// </returns>
        public PriorityBlending Add(IMovement movement, float priority)
        {
            _blenders.Add((movement, priority));
            _blenders = _blenders
                .OrderBy(b => b.Priority)
                .ToList();
            return this;
        }

        public Vector2 GetDirection()
        {
            foreach (var blender in _blenders)
            {
                var direction = blender.Movement.GetDirection();
                if (direction.magnitude > 0.5)
                {
                    return direction;
                }
            }
            return Vector2.zero;
        }
    }
}