using UnityEngine;
using brazenhead.Core;

namespace brazenhead
{
    internal class Rider : MonoBehaviour, ITickable<PhysicsManager.ILoop>
    {
        [SerializeField] private GameEntityRiderSettings _settings;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;

        protected async void OnEnable()
        {
            while (!Game.Loops.TryAddTickable(this))
                await Awaitable.NextFrameAsync();
        }

        protected void OnDisable()
        {   

            Game.Loops.TryRemoveTickable(this);
        }

        void ITickable<PhysicsManager.ILoop>.OnTick(in float deltaTime, in float alpha)
        {
            Debug.Log("OnTick: " + deltaTime + " - " + alpha);
        }
    }
}
