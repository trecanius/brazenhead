using System;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal class UnityInputActions : InputActions
    {
        public void Initialize(InputSystemActions actions)
        {
            //Move = new UnityInputAction<Vector2>(actions.Player.Move);
            //Attack = new UnityInputAction<float>(actions.Player.Attack);
        }
    }
}
