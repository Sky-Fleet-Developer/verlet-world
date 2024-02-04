using System.Collections.Generic;
using Bones;
using Leopotam.Ecs;

namespace GridEditor
{
    public class GridAdaptorSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SkeletonAdaptor _adaptor;
        private Graph _sourceGraph;

        public GridAdaptorSystem(Graph graph)
        {
            _sourceGraph = graph;
        }
        public void Init()
        {
            Dictionary<int, int> nodesMap = new ();
            foreach (Node node in _sourceGraph.Nodes.Values)
            {
                int newIdx = _adaptor.CreateNode(node.Position);
                nodesMap.Add(node.Id, newIdx);
            }

            foreach (Edge edge in _sourceGraph.Edges)
            {
                CreateEdge(nodesMap[edge.NodeA], nodesMap[edge.NodeB]);
            }
        }
        
        private void CreateEdge(int nodeA, int nodeB)
        {
            EcsEntity entity = _world.NewEntity();
            entity.Replace(new EdgeComponent {NodeA = nodeA, NodeB = nodeB, Spring = 70, Dumper = 30});
        }
    }
}