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

        
        public bool TryAddEdge<T>(int nodeA, int nodeB, out T edge) where T : Edge, new()
        {
            if (nodeA == nodeB)
            {
                edge = null;
                return false;
            }
            Edge newEdge = new T();
            newEdge.NodeA = nodeA;
            newEdge.NodeB = nodeB;
            if (Edges.Contains(newEdge))
            {
                edge = (T)Edges.First(x => x.Equals(newEdge));
                return false;
            }

            Edges.Add(newEdge);
            edge = (T)newEdge;
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
            IEnumerable<Edge> connected = GetConnectedEdges(node);
            Edges = Edges.Except(connected).ToList();
            IEnumerable<int> otherNodes = connected.Select(x => x.NodeA == node ? x.NodeB : x.NodeA);
            foreach (int otherNode in otherNodes)
            {
                if (!GetConnectedEdges(otherNode).Any())
                {
                    RemoveNode(otherNode, false);
                }
            }
        }

        public float GetDistanceBetweenNodes(int nodeA, int nodeB)
        {
            return Vector3.Distance(Nodes[nodeA].Position, Nodes[nodeB].Position);
        }
    }
}