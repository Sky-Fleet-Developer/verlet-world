using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridEditor
{
    public class Graph
    {
        public readonly Dictionary<int, Node> Nodes = new();
        public List<Edge> Edges { get; private set; } = new ();

        public Node AddNode(Vector3 position)
        {
            Node newNode = new Node();
            Nodes.Add(newNode.Id, newNode);
            newNode.Position = position;
            return newNode;
        }

        
        public bool TryAddEdge(int nodeA, int nodeB, out Edge edge)
        {
            Edge newEdge = new Edge();
            newEdge.NodeA = nodeA;
            newEdge.NodeB = nodeB;
            if (Edges.Contains(newEdge))
            {
                edge = Edges.First(x => x.Equals(newEdge));
                return false;
            }

            Edges.Add(newEdge);
            edge = newEdge;
            return true;
        }
        
        public Node GetClosestNode(Vector3 point, out float sqrDistance, float maxDistance = 10e6f)
        {
            Node selected = null;
            float sqrDist = maxDistance * maxDistance;
            foreach (Node node in Nodes.Values)
            {
                float d = Vector3.SqrMagnitude(node.Position - point);
                if (d < sqrDist)
                {
                    sqrDist = d;
                    selected = node;
                }
            }
            sqrDistance = sqrDist;
            return selected;
        }

        public IEnumerable<Edge> GetConnectedEdges(int node)
        {
            return Edges.Where(x => x.NodeA == node || x.NodeB == node);
        }

        public void RemoveNode(int node, bool recursively = true)
        {
            if (recursively)
            {
                RemoveConnectedEdges(node);
            }
            Nodes.Remove(node);
        }

        public void RemoveEdge(Edge edge)
        {
            Edges.Remove(edge);
        }
        public void RemoveConnectedEdges(int node)
        {
            Edges = Edges.Except(GetConnectedEdges(node)).ToList();
        }
    }
}