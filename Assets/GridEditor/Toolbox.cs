using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridEditor
{
    public class Toolbox
    {
        private List<Tool> _tools;
        private int _selectedTool;
        public readonly Graph Graph;
        
        public Toolbox(Graph graph, params Type[] toolTypes)
        {
            Graph = graph;
            _tools = new List<Tool>();
            foreach (Type toolType in toolTypes)
            {
                _tools.Add((Tool)Activator.CreateInstance(toolType, this));
            }
        }

        public void Init()
        {
            foreach (Tool tool in _tools)
            {
                tool.Init();   
            }
        }
        
        public T GetTool<T>() where T : Tool
        {
            return _tools.FirstOrDefault(x => x is T) as T;
        }

        public void OnMouseUp(Vector3 position, int keyIdx)
        {
            _tools[_selectedTool].OnMouseUp(position, new ClickParams{IsAlternative = keyIdx != 0});
        }
    }
}