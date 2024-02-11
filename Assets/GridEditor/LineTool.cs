using System;
using System.Linq;
using UnityEngine;

namespace GridEditor
{
    [Serializable]
    public class LineTool : Tool
    {
        private SelectionTool _selectionTool;
        private bool _createLeverEdge = false;
        public KeyCode LeverKey;
        public LineTool(Toolbox toolbox) : base(toolbox)
        {
        }

        public override void Init()
        {
            _selectionTool = Toolbox.GetTool<SelectionTool>();
        }


        public override void OnGui()
        {
            _createLeverEdge = Input.GetKey(KeyCode.LeftAlt);
            base.OnGui();
        }

        public override void OnMouseUp(Vector3 position, ClickParams clickParams)
        {
            base.OnMouseUp(position, clickParams);

            if (DragDelta.sqrMagnitude < GraphEditor.AnchorDist)
            {
                
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
                    else if (ClosestNode != null)
                    {
                        Toolbox.Graph.RemoveNode(ClosestNode.Id);
                    }
                    return;
                }
                

                if (ClosestNode == null)
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
                        Enqueue(ClosestNode.Id);
                        return;
                    }
                    else
                    {
                        Step(ClosestNode.Id);
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
            if (TryAddEdge(lastNode, nodeId, out _))
            {
                _selectionTool.SelectedNodes.Dequeue();
                _selectionTool.SelectedNodes.Enqueue(nodeId);
            }
            else
            {
                Debug.Log("Operation cancelled: add new edge. Course: already exists");
            }
        }

        private bool TryAddEdge(int nodeA, int nodeB, out Edge edge)
        {
            if (_createLeverEdge)
            {
                if (Toolbox.Graph.TryAddEdge(nodeA, nodeB, out LeverEdge leverEdge))
                {
                    leverEdge.Key = LeverKey;
                    leverEdge.ActivatedLengthPercent = 0.4f;
                    edge = leverEdge;
                    return true;
                }
            }
            else
            {
                if (Toolbox.Graph.TryAddEdge(nodeA, nodeB, out edge))
                {
                    return true;
                }
            }

            edge = null;
            return false;
        }
    }
}