using System.Linq;
using UnityEngine;

namespace GridEditor
{
    public class LineTool : Tool
    {
        private SelectionTool _selectionTool;
        public LineTool(Toolbox toolbox) : base(toolbox)
        {
        }

        public override void Init()
        {
            _selectionTool = Toolbox.GetTool<SelectionTool>();
        }

        public override void OnMouseUp(Vector3 position, ClickParams clickParams)
        {
            base.OnMouseUp(position, clickParams);

            if (DragDelta.sqrMagnitude < 0.1f)
            {
                Node closestNode = Toolbox.Graph.GetClosestNode(position, out _, GraphEditor.AnchorDist);
                
                if (clickParams.IsAlternative)
                {
                    if (_selectionTool.SelectedNodes.TryDequeue(out int lastNode))
                    {
                        if (!Toolbox.Graph.GetConnectedEdges(lastNode).Any())
                        {
                            Toolbox.Graph.RemoveNode(lastNode, false);
                        }
                        _selectionTool.SelectedNodes.Clear();
                    }
                    else
                    {
                        Toolbox.Graph.RemoveNode(closestNode.Id);
                    }
                    return;
                }
                

                if (closestNode == null)
                {
                    if (_selectionTool.SelectedNodes.Count == 0)
                    {
                        Enqueue(position);
                        return;
                    }
                    else
                    {
                        Step(position);
                        return;
                    }
                }
                else
                {
                    if (_selectionTool.SelectedNodes.Count == 0)
                    {
                        Enqueue(closestNode.Id);
                        return;
                    }
                    else
                    {
                        Step(closestNode.Id);
                        return;
                    }
                }
            }
        }

        private void Enqueue(Vector3 position)
        {
            Node newNode = Toolbox.Graph.AddNode(position);
            Enqueue(newNode.Id);
        }

        private void Step(Vector3 position)
        {
            Node newNode = Toolbox.Graph.AddNode(position);
            Step(newNode.Id);
        }

        private void Enqueue(int nodeId)
        {
            _selectionTool.SelectedNodes.Enqueue(nodeId);
        }

        private void Step(int nodeId)
        {
            int lastNode = _selectionTool.SelectedNodes.Peek();
            if (Toolbox.Graph.TryAddEdge(lastNode, nodeId, out _))
            {
                _selectionTool.SelectedNodes.Dequeue();
                _selectionTool.SelectedNodes.Enqueue(nodeId);
            }
            else
            {
                Debug.Log("Operation cancelled: add new edge. Course: already exists");
            }
        }
    }
}