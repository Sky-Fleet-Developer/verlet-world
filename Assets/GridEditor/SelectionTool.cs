using System.Collections.Generic;

namespace GridEditor
{
    public class SelectionTool : Tool
    {
        public readonly Queue<int> SelectedNodes = new ();
        public SelectionTool(Toolbox toolbox) : base(toolbox)
        {
        }

        public bool IsNodeSelected(int id)
        {
            return SelectedNodes.Contains(id);
        }
    }
}