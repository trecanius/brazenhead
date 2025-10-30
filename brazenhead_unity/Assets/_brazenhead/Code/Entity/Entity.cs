using UnityEngine;

namespace brazenhead
{
    internal class Entity
    {
        internal EntitySceneObject SceneObject { get; private set; }

        internal Entity(EntitySceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }
    }
}
