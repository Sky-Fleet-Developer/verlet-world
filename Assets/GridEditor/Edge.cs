using System;
using System.Collections.Generic;
using Bones;
using Bones.Components;
using Leopotam.Ecs;

namespace GridEditor
{
    public class Edge : Identifiable, IEquatable<Edge>
    {
        public int NodeA;
        public int NodeB;
        public bool Equals(Edge other)
        {
            if (other == null) return false;
            return other.NodeA == NodeA && other.NodeB == NodeB || other.NodeA == NodeB && other.NodeB == NodeA;
        }

        public virtual void CreateEdgeComponent(EcsEntity entity, Dictionary<int, int> nodesMap,  Graph graph)
        {
            entity.Replace(new EdgeComponent {NodeA = nodesMap[NodeA], NodeB = nodesMap[NodeB], Spring = 10000, Dumper = 500, Length = graph.GetDistanceBetweenNodes(NodeA, NodeB)});
        }
    }
}