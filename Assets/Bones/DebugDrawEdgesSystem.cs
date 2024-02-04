using Leopotam.Ecs;
using UnityEngine;

namespace Bones
{
    public class DebugDrawEdgesSystem : IEcsRunSystem
    {
        private EcsFilter<NodeComponent> _nodesFilter; 
        private EcsFilter<EdgeComponent> _edgesFilter; 
        public void Run()
        {
            foreach (int i in _edgesFilter)
            {
                EdgeComponent edgeComponent = _edgesFilter.Get1(i);
                NodeComponent nodeA = _nodesFilter.Get1(edgeComponent.NodeA);
                NodeComponent nodeB = _nodesFilter.Get1(edgeComponent.NodeB);
                Debug.DrawLine(nodeA.Position, nodeB.Position);
            }
        }
    }
}