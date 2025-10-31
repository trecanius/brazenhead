using brazenhead.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace brazenhead
{
    internal class PhysicsManager : IListener<Game.Start>, IListener<Game.Stop>, ITickable<PhysicsManager.ILoop>
    {
        private float _timestep = 1f / 30f;
        private float _accumulator;

        private readonly List<ITickable> _tickables = new();
        private readonly List<Collider> _colliders = new();

        internal PhysicsManager()
        {
            Physics.simulationMode = SimulationMode.Script;

            Game.EventBus.AddListener<Game.Start>(this);
            Game.EventBus.AddListener<Game.Stop>(this);
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            Game.Loops.Add<ILoop>(0f, -1).AddTickable(this, int.MinValue);

            var tickRateSetting = Game.Locator.Resolve<ConfigManager>().Settings.PhysicsTickRate;
            tickRateSetting.ValueSet += OnTickRateValueSet;
            OnTickRateValueSet(tickRateSetting.GetValue());

            GetAllColliders();

            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        void IListener<Game.Stop>.OnEvent(in Game.Stop param)
        {
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        void ITickable<ILoop>.OnTick(in float deltaTime)
        {
            // only simulate whole steps
            _accumulator += deltaTime;
            while (_accumulator >= _timestep)
            {
                _accumulator -= _timestep;
                Physics.Simulate(_timestep);
                foreach (var tickable in _tickables)
                    tickable.OnPhysicsTick(deltaTime);
            }
            float alpha = _accumulator / _timestep;
            foreach (var tickable in _tickables)
                tickable.OnFrameTick(deltaTime, alpha);
            //Debug.Log("OnTick - frame: " + Time.frameCount + " dt: " + deltaTime + " alpha: " + alpha);
        }

        internal void AddTickable(ITickable tickable)
        {
            _tickables.Add(tickable);
        }

        internal void RemoveTickable(ITickable tickable)
        {
            _tickables.Remove(tickable);
        }

        private void OnTickRateValueSet(in float value)
        {
            _timestep = 1f / value;
        }

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            GL.PushMatrix();
            //GL.LoadIdentity();
            Game.Locator.Resolve<AssetCatalog.MaterialRefs>().DebugLine.SetPass(0);
            GL.Begin(GL.LINE_STRIP);
            foreach (var collider in _colliders)
            {
                if (collider is BoxCollider box)
                {
                    var bounds = box.bounds;
                    var center = bounds.center;
                    var extents = bounds.extents;
                    GL.Vertex(center + new Vector3(extents.x, extents.y, extents.z));
                    GL.Vertex(center + new Vector3(-extents.x, extents.y, extents.z));
                    GL.Vertex(center + new Vector3(-extents.x, extents.y, -extents.z));
                    GL.Vertex(center + new Vector3(extents.x, extents.y, -extents.z));
                    GL.Vertex(center + new Vector3(-extents.x, -extents.y, -extents.z));
                    GL.Vertex(center + new Vector3(extents.x, -extents.y, -extents.z));
                    GL.Vertex(center + new Vector3(extents.x, -extents.y, extents.z));
                    GL.Vertex(center + new Vector3(-extents.x, -extents.y, extents.z));
                }
            }
            GL.End();
            GL.PopMatrix();
        }

        private void GetAllColliders()
        {
            var rootGameObjects = new List<GameObject>();
            for (int i = UnityEngine.SceneManagement.SceneManager.sceneCount - 1; i >= 0; i--)
                UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects(rootGameObjects);

            _colliders.Clear();
            foreach (var go in rootGameObjects)
                _colliders.AddRange(go.GetComponentsInChildren<Collider>());
        }

        internal interface ITickable
        {
            internal void OnPhysicsTick(in float deltaTime);
            internal void OnFrameTick(in float deltaTime, in float alpha);
        }

        private interface ILoop { }
    }
}
