using UnityEngine;

namespace Bones.Components
{
    public struct NodeComponent
    {
        public Vector3 Position;
        public bool IsStatic;
        public NodeComponent(Vector3 position)
        {
            Position = position;
            IsStatic = false;
        }
    }
}