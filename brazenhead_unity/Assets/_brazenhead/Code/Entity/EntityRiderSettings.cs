using UnityEngine;

namespace brazenhead
{
    [CreateAssetMenu(fileName = "GameEntityMovementSettings", menuName = "brazenhead/GameEntity/MovementSettings")]
    internal class GameEntityRiderSettings : ScriptableObject
    {
        [field: SerializeField] internal float RayLength { get; private set; }
        [field: SerializeField] internal float RestHeight { get; private set; }
        [field: SerializeField] internal float SpringStrength { get; private set; }
        [field: SerializeField] internal float SpringDamper { get; private set; }
        [field: SerializeField] internal float Speed { get; private set; }
    }
}
