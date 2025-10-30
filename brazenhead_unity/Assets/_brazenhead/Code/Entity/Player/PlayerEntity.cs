using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class PlayerEntity : Entity
    {
        [field: SerializeField] internal Transform CameraHolder { get; private set; }

        internal PlayerEntity(EntitySceneObject sceneObject) : base(sceneObject)
        {
            var inputActions = Game.Locator.Resolve<InputManager>().Actions;
            inputActions.Move.StateChanged += OnMoveInputValueChanged;
            inputActions.Attack.StateChanged += OnAttackInputValueChanged;
        }

        private void OnMoveInputValueChanged(InputAction<Vector2>.State state, Vector2 value)
        {
            Debug.Log(Time.frameCount + " - Move - " + state + " - " + value);
        }

        private void OnAttackInputValueChanged(InputAction<float>.State state, float value)
        {
            Debug.Log(Time.frameCount + " - Attack - " + state + " - " + value);
        }
    }
}
