using UnityEngine;

namespace brazenhead
{
    [CreateAssetMenu(fileName = "GameEntityMovementSettings", menuName = "brazenhead/GameEntity/MovementSettings")]
    internal class GameEntityMovementSettings : ScriptableObject
    {
        [field: SerializeField] internal float Speed { get; private set; }
    }
}
