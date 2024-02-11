using System;
using UnityEngine;

namespace GridEditor
{
    [Serializable]
    public abstract class Tool
    {
        public Toolbox Toolbox { get; }
        public Vector3 StartDragPosition { get; private set; }
        public Vector3 DragDelta { get; private set; }
        public Vector3 MousePosition { get; private set; }
        public Node ClosestNode { get; private set; }
        public virtual string Name
        {
            get => GetType().Name;
        }
        public Tool(Toolbox toolbox)
        {
            Toolbox = toolbox;
        }

        protected virtual void OnActivated()
        {
        }
        protected virtual void OnDeactivated()
        {
        }
        
        public virtual void Init(){}

        public virtual void OnMouseDown(Vector3 position, ClickParams clickParams)
        {
            ClosestNode = Toolbox.Graph.GetClosestNode(position, out _, GraphEditor.AnchorDist);
            
            StartDragPosition = ClosestNode?.Position ?? position;
            DragDelta = Vector3.zero;
            MousePosition = StartDragPosition;
        }

        /*public virtual void OnMouseDrag(Vector3 delta, ClickParams clickParams)
        {
            DragDelta += delta;
            MousePosition += delta;
        }*/
        
        public virtual void OnMouseUp(Vector3 position, ClickParams clickParams)
        {
            ClosestNode = Toolbox.Graph.GetClosestNode(position, out _, GraphEditor.AnchorDist);
            if (ClosestNode != null)
            {
                position = ClosestNode.Position;
            }
            DragDelta = position - StartDragPosition;
            MousePosition = position;
        }

        public virtual void OnGui()
        {
            GUILayout.Label($"Selected tool: {GetType().Name}");
        }
    }
}