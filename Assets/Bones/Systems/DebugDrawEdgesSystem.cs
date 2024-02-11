using Bones.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Bones.Systems
{
    public class DebugDrawEdgesSystem : IEcsRunSystem
    {
        private EcsFilter<NodeComponent> _nodesFilter;
        private EcsFilter<EdgeComponent> _edgesFilter;
        private EcsFilter<EdgeComponent, EdgeLeverComponent> _leversFilter;
        public void Run()
        {
            foreach (int i in _edgesFilter)
            {
                EdgeComponent edge = _edgesFilter.Get1(i);
                NodeComponent nodeA = _nodesFilter.Get1(edge.NodeA);
                NodeComponent nodeB = _nodesFilter.Get1(edge.NodeB);
                Debug.DrawLine(nodeA.Position, nodeB.Position);
            }

            foreach (int i in _leversFilter)
            {
                EdgeComponent edge = _leversFilter.Get1(i);
                NodeComponent nodeA = _nodesFilter.Get1(edge.NodeA);
                NodeComponent nodeB = _nodesFilter.Get1(edge.NodeB);
                EdgeLeverComponent lever = _leversFilter.Get2(i);
                Vector3 cross = Vector3.Cross(nodeA.Position - nodeB.Position, Vector3.forward) * 0.05f;
                
                Debug.DrawLine(nodeA.Position + cross, nodeB.Position + cross, Color.cyan * 0.7f);
                Debug.DrawLine(nodeA.Position - cross, nodeB.Position - cross, Color.cyan * 0.7f);
            }
        }
    }
}