using brazenhead;
using UnityEngine;

namespace brazenhead
{
    internal class PlayerEntity : Entity
    {
        Rider _rider;

        internal PlayerEntity(EntitySceneObject sceneObject) : base(sceneObject)
        {
            var inputActions = GameSession.Instance.Locator.Resolve<InputManager>().Actions;
            inputActions.Move.StateChanged += OnMoveInputValueChanged;
            inputActions.Attack.StateChanged += OnAttackInputValueChanged;

            var cameraHolder = sceneObject.transform.Find("Rider/Rigidbody/CameraHolder");
            GameSession.Instance.Locator.Resolve<CameraManager>().MainCamera.transform.SetParent(cameraHolder, false);

            _rider = sceneObject.GetComponentInChildren<Rider>();
            GameSession.Instance.Locator.Resolve<PhysicsManager>().AddTickable(_rider);
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
