using UnityEngine;

namespace brazenhead
{
    internal class UnityInputActions : InputActions
    {
        internal UnityInputActions(InputSystemActions actions)
        {
            Move = new UnityInputAction<Vector2>(actions.Player.Move);
            Attack = new UnityInputAction<float>(actions.Player.Attack);
        }
    }
}
