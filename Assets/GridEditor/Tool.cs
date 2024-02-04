using UnityEngine;

namespace GridEditor
{
    public abstract class Tool
    {
        public Toolbox Toolbox { get; }
        public Vector3 StartDragPosition { get; private set; }
        public Vector3 DragDelta { get; private set; }
        public Vector3 MousePosition { get; private set; }
        public Tool(Toolbox toolbox)
        {
            Toolbox = toolbox;
        }
        
        public virtual void Init(){}

        public virtual void OnMouseDown(Vector3 position, ClickParams clickParams)
        {
            StartDragPosition = position;
            DragDelta = Vector3.zero;
            MousePosition = position;
        }

        public virtual void OnMouseDrag(Vector3 delta, ClickParams clickParams)
        {
            DragDelta += delta;
            MousePosition += delta;
        }
        
        public virtual void OnMouseUp(Vector3 position, ClickParams clickParams)
        {
            MousePosition = position;
        }
    }
}