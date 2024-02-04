using System;
using System.Collections.Generic;
using Bones;
using Canvas;
using UnityEngine;
using Zenject;

namespace GridEditor
{
    public class GraphEditor : MonoBehaviour
    {
        public const float AnchorDist = 0.3f;
        [SerializeField] private SkeletonAdaptor skeleton;
        [Inject] private WorldSpaceInput _worldSpaceInput;
        private Toolbox _toolbox = new Toolbox(new Graph(), typeof(LineTool), typeof(SelectionTool));
        private bool _isEditorActive = false;
        void Start()
        {
            _toolbox.Init();
            skeleton.gameObject.SetActive(false);
            InitSubscriptions();
            _isEditorActive = true;
        }

        private void InitSubscriptions()
        {
            _worldSpaceInput.MouseUp += OnMouseButtonUp;
        }

        private void RemoveSubscriptions()
        {
            _worldSpaceInput.MouseUp -= OnMouseButtonUp;
        }

        private void OnMouseButtonUp(int idx)
        {
            _toolbox.OnMouseUp(_worldSpaceInput.MousePosition, idx);
        }

        private void OnDrawGizmos()
        {
            if(!_isEditorActive) return;
            
            foreach (Edge edge in _toolbox.Graph.Edges)
            {
                Node nodeA = _toolbox.Graph.Nodes[edge.NodeA];
                Node nodeB = _toolbox.Graph.Nodes[edge.NodeB];
                Debug.DrawLine(nodeA.Position, nodeB.Position, Color.green);
            }

            Queue<int> selected = _toolbox.GetTool<SelectionTool>().SelectedNodes;
            if (selected.Count > 0)
            {
                Node node = _toolbox.Graph.Nodes[selected.Peek()];
                Debug.DrawLine(node.Position, _worldSpaceInput.MousePosition, Color.red);
            }

            Vector3 pointer = _worldSpaceInput.MousePosition;
            var closestNode = _toolbox.Graph.GetClosestNode(_worldSpaceInput.MousePosition, out _, AnchorDist);
            if (closestNode != null)
            {
                pointer = closestNode.Position;
            }
            
            Debug.DrawLine(pointer + Vector3.right * 0.1f, pointer - Vector3.right * 0.1f, Color.blue);
            Debug.DrawLine(pointer + Vector3.up * 0.1f, pointer - Vector3.up * 0.1f, Color.blue);
        }

        private void OnGUI()
        {
            if (_isEditorActive)
            {
                if (GUILayout.Button("Construct"))
                {
                    skeleton.gameObject.SetActive(true);
                    skeleton.AddSystemDelayed(0, new GridAdaptorSystem(_toolbox.Graph));
                    skeleton.Init();
                    _isEditorActive = false;
                    RemoveSubscriptions();
                }
            }
            else
            {
                if (GUILayout.Button("Edit"))
                {
                    skeleton.DestroyWorld();
                    skeleton.gameObject.SetActive(false);
                    _isEditorActive = true;
                    InitSubscriptions();
                }
            }
        }
    }
}