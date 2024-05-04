using System;
using System.Collections.Generic;
using Bones;
using Canvas;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GridEditor
{
    public class GraphEditor : MonoBehaviour
    {
        public const float AnchorDist = 0.3f;
        [SerializeField] private GraphToRigidAdaptor skeleton;
        [Inject] private WorldSpaceInput _worldSpaceInput;
        [ShowInInspector] private Toolbox _toolbox = new Toolbox(new Graph(), typeof(LineTool), typeof(SelectionTool), typeof(ShapeTool));
        private bool _isEditorActive = false;
        private Camera camera;
        void Start()
        {
            _toolbox.Init();
            skeleton.gameObject.SetActive(false);
            InitSubscriptions();
            _isEditorActive = true;
            camera = Camera.main;
        }

        private void InitSubscriptions()
        {
            _worldSpaceInput.MouseUp += OnMouseButtonUp;
            _worldSpaceInput.MouseDown += OnMouseDown;
        }

        private void RemoveSubscriptions()
        {
            _worldSpaceInput.MouseUp -= OnMouseButtonUp;
        }

        private void OnMouseButtonUp(int idx)
        {
            _toolbox.OnMouseUp(_worldSpaceInput.MousePosition, idx);
        }
        private void OnMouseDown(int idx)
        {
            _toolbox.OnMouseDown(_worldSpaceInput.MousePosition, idx);
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
            Vector3 screenZero = camera.WorldToScreenPoint(Vector3.zero);
            GUILayout.BeginArea(new Rect(0, Screen.height - screenZero.y, Screen.width, screenZero.y));
            if (_isEditorActive)
            {
                if (GUILayout.Button("Construct", GUILayout.Width(200)))
                {
                    skeleton.gameObject.SetActive(true);
                    //skeleton.AddSystemDelayed(0, new GridAdaptorSystem(_toolbox.Graph));
                    //skeleton.Init();
                    skeleton.ConstructFromGraph(_toolbox.Graph);
                    _isEditorActive = false;
                    RemoveSubscriptions();
                }
                _toolbox.OnGui();
            }
            else
            {
                if (GUILayout.Button("Edit", GUILayout.Width(200)))
                {
                    skeleton.DestroyWorld();
                    skeleton.gameObject.SetActive(false);
                    _isEditorActive = true;
                    InitSubscriptions();
                }
            }
            GUILayout.EndArea();
        }
    }
}