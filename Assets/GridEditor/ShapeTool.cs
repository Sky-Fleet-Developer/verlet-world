using System.Linq;
using UnityEngine;

namespace GridEditor
{
    public class ShapeTool : Tool
    {
        private readonly IntFieldGui _cornersAmount = new IntFieldGui("Corners:", 4);
        private SelectionTool _selectionTool;
        private Node _centralNode;
        public ShapeTool(Toolbox toolbox) : base(toolbox)
        {
        }

        public override void Init()
        {
            _selectionTool = Toolbox.GetTool<SelectionTool>();
        }

        public override void OnMouseDown(Vector3 position, ClickParams clickParams)
        {
            base.OnMouseDown(position, clickParams);
            _centralNode = ClosestNode;
            if (_centralNode == null)
            {
                _centralNode = Toolbox.Graph.AddNode(position);
            }
        }

        public override void OnMouseUp(Vector3 position, ClickParams clickParams)
        {
            base.OnMouseUp(position, clickParams);
            if (DragDelta.sqrMagnitude > GraphEditor.AnchorDist)
            {
                if (clickParams.IsAlternative)
                {
                    if (_centralNode != null && !Toolbox.Graph.GetConnectedEdges(_centralNode.Id).Any())
                    {
                        Toolbox.Graph.RemoveNode(_centralNode.Id, false);
                    }
                    return;
                }

                float radius = DragDelta.magnitude;
                float startDeg = Mathf.Atan2(DragDelta.y, DragDelta.x);
                Node[] circle = new Node[_cornersAmount.Value];
                for (int i = 0; i < _cornersAmount.Value; i++)
                {
                    float angle = startDeg + (float)i / _cornersAmount.Value * Mathf.PI * 2;
                    circle[i] = Toolbox.Graph.AddNode(StartDragPosition + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0));
                }

                for (int i = 0; i < circle.Length - 1; i++)
                {
                    Toolbox.Graph.TryAddEdge(circle[i].Id, circle[i+1].Id, out Edge _);
                }
                Toolbox.Graph.TryAddEdge(circle[0].Id, circle[^1].Id, out Edge _);
            }
        }

        public override void OnGui()
        {
            _cornersAmount.DrawGUIElement();
        }
    }
}