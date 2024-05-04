using GridEditor;
using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

public class GraphToRigidAdaptor : MonoBehaviour
{
    private List<Rigidbody2D> nodes = new List<Rigidbody2D>();
    private Dictionary<int, int> nodesIdxs = new ();
    private List<(int, int)> edges = new();

    public void ConstructFromGraph(Graph graph)
    {

        foreach (Node node in graph.Nodes.Values)
        {
            var nodeBody = CreateNode(node);
            nodesIdxs.Add(node.Id, nodes.Count);
            nodes.Add(nodeBody);
        }

        foreach (Edge edge in graph.Edges)
        {
            //edge.CreateEdgeComponent(entity, nodesMap, _sourceGraph);
            CreateEdge(nodes[nodesIdxs[edge.NodeA]], nodes[nodesIdxs[edge.NodeB]]);
            edges.Add((edge.NodeA, edge.NodeB));
        }
    }

    private void Update()
    {
        foreach (var edge in edges)
        {
            Debug.DrawLine(nodes[nodesIdxs[edge.Item1]].transform.position, nodes[nodesIdxs[edge.Item2]].transform.position);
        }
    }

    private Rigidbody2D CreateNode(Node nodeSource)
    {
        GameObject nodeObject = new GameObject($"Node {nodeSource.Id}", typeof(Rigidbody2D), typeof(CircleCollider2D));
        nodeObject.transform.position = nodeSource.Position;
        var rigidbody = nodeObject.GetComponent<Rigidbody2D>();
        rigidbody.mass = 1;
        rigidbody.angularDrag = 0;
        rigidbody.freezeRotation = true;
        var collider = nodeObject.GetComponent<CircleCollider2D>();
        collider.radius = 0.1f;
        return rigidbody;
    }

    private void CreateEdge(Rigidbody2D nodeA, Rigidbody2D nodeB)
    {
        var joint = nodeA.gameObject.AddComponent<SpringJoint2D>();
        joint.connectedBody = nodeB;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector2.zero;
        joint.autoConfigureDistance = false;
        joint.distance = Vector3.Distance(nodeA.transform.position, nodeB.transform.position);
        joint.frequency = 5;
        joint.dampingRatio = 0.05f;
    }

    public void DestroyWorld()
    {
        foreach (var item in nodes)
        {
            Destroy(item.gameObject);
        }
        nodes.Clear();
        edges.Clear();
        nodesIdxs.Clear();
    }
}
