using System.Collections.Generic;
using Bones;
using Bones.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace GridEditor
{
    public class LeverEdge : Edge
    {
        public KeyCode Key;
        public float ActivatedLengthPercent;

        public override void CreateEdgeComponent(EcsEntity entity, Dictionary<int, int> nodesMap, Graph graph)
        {
            float length = graph.GetDistanceBetweenNodes(NodeA, NodeB);
            entity.Replace(new EdgeComponent {NodeA = nodesMap[NodeA], NodeB = nodesMap[NodeB], Spring = 700, Dumper = 50, Length = length});
            entity.Replace(new EdgeLeverComponent() {DefaultLengthValue = length, Key = Key, ActivatedLengthPercent = ActivatedLengthPercent});
        }
    }
}