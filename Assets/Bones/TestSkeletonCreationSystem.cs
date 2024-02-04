using Leopotam.Ecs;
using UnityEngine;

namespace Bones
{
    public class TestSkeletonCreationSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SkeletonAdaptor _adaptor;
        public void Init()
        {
            int nodeAIdx = _adaptor.CreateNode(Vector3.left * 2);
            int nodeBIdx = _adaptor.CreateNode(Vector3.right * 2);
            int nodeCIdx = _adaptor.CreateNode(Vector3.up * 3);
            CreateEdge(nodeAIdx, nodeBIdx);
            CreateEdge(nodeBIdx, nodeCIdx);
            CreateEdge(nodeCIdx, nodeAIdx);
            int tailA = _adaptor.CreateNode(Vector3.up * 2.3f);
            int tailB = _adaptor.CreateNode(Vector3.up * 1.6f);
            int tailC = _adaptor.CreateNode(Vector3.up * 1f);
            CreateEdge(nodeCIdx, tailA);
            CreateEdge(tailA, tailB);
            CreateEdge(tailB, tailC);
        }

        private void CreateEdge(int nodeA, int nodeB)
        {
            EcsEntity entity = _world.NewEntity();
            entity.Replace(new EdgeComponent {NodeA = nodeA, NodeB = nodeB, Spring = 70, Dumper = 30});
        }
    }
}
