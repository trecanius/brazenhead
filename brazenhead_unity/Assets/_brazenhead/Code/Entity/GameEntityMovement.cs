using brazenhead.Core;
using System;
using UnityEngine;

namespace brazenhead
{
    internal partial class GameEntity
    {
        internal class Movement
        {
            private readonly GameEntityMovementSettings _settings;

            private Vector2 _movementVector;

            internal Movement(in GameEntityMovementSettings settings)
            {
                _settings = settings;
            }

            internal void SetMovementVector(Vector2 vector)
            {
                _movementVector = vector;
            }
        }
    }
}
