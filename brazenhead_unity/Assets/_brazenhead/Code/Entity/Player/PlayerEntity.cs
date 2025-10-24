using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class PlayerEntity : GameEntity
    {
        [field: SerializeField] internal Transform CameraHolder { get; private set; }

        internal override void Initialize()
        {
            base.Initialize();

            var inputActions = Game.Locator.Resolve<InputActions>();
            inputActions.Move.Performed += OnMoveInputValueChanged;
            inputActions.Attack.Performed += OnAttackInputValueChanged;
        }

        private void OnMoveInputValueChanged(in Vector2 value)
        {
            Debug.Log(value);
        }

        private void OnAttackInputValueChanged(in float value)
        {
            Debug.Log(value);
        }
    }
}
