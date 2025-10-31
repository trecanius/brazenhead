using UnityEngine;

namespace brazenhead
{
    internal class Rider : MonoBehaviour, PhysicsManager.ITickable
    {
        [SerializeField] private GameEntityRiderSettings _settings;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;

        private Vector3 _velocity;

        void PhysicsManager.ITickable.OnPhysicsTick(in float deltaTime)
        {
            var transform = _rigidbody.transform;
            var downDir = -transform.up;
            var rayDir = transform.TransformDirection(downDir);
            var ray = new Ray(transform.position + new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f));
            bool result = Physics.Raycast(ray, out var hitInfo, _settings.RayLength);

            if (result)
            {
                var velocity = _rigidbody.linearVelocity;
                var otherVelocity = Vector3.zero;
                var rayDirVel = Vector3.Dot(ray.direction, velocity);
                var otherDirVel = Vector3.Dot(ray.direction, otherVelocity);

                float relVel = rayDirVel - otherDirVel;
                float x = hitInfo.distance - _settings.RestHeight;
                float springForce = (x * _settings.SpringStrength) - (relVel * _settings.SpringDamper);

                _rigidbody.AddForce(ray.direction * springForce);
            }
        }

        void PhysicsManager.ITickable.OnFrameTick(in float deltaTime, in float alpha)
        {
        }
    }
}
