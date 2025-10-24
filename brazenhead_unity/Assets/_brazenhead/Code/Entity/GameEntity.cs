using UnityEngine;

namespace brazenhead
{
    internal partial class GameEntity : MonoBehaviour
    {
        [SerializeField] private GameEntityMovementSettings _movementSettings;

        private Movement _movement;

        internal virtual void Initialize()
        {
            _movement = new(_movementSettings);
        }
    }
}
