using UnityEngine;

namespace brazenhead
{
    internal class GameBehaviour : MonoBehaviour
    {
        internal Collider[] Colliders { get; private set; }

        protected void Awake()
        {
            Colliders = GetComponentsInChildren<Collider>(true);
        }
    }
}
