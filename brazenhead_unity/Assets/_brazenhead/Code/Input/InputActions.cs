using UnityEngine;

namespace brazenhead
{
    internal class InputActions
    {
        internal InputAction<Vector2> Move { get; }
        internal InputAction<float> Attack { get; }

        internal InputActions(UnityInputActions actions)
        {
            Move = new(actions.Player.Move);
            Attack = new(actions.Player.Attack);
        }
    }
}
