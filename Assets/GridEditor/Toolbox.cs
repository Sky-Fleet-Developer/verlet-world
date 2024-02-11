using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GridEditor
{
    [Serializable]
    public class Toolbox
    {
        [ShowInInspector] private List<Tool> _tools;
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
        
        public void OnMouseDown(Vector3 position, int keyIdx)
        {
            _tools[_selectedTool].OnMouseDown(position, new ClickParams{IsAlternative = keyIdx != 0});
        }
        
        /*public void OnMouseDrag(Vector3 position, int keyIdx)
        {
            _tools[_selectedTool].OnMouseDrag(position, new ClickParams{IsAlternative = keyIdx != 0});
        }*/
        
        
        public void OnGui()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(400));
            GUILayout.BeginVertical(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(200));
            DrawToolSelection();
            GUILayout.EndVertical();
            _tools[_selectedTool].OnGui();
            GUILayout.EndHorizontal();
        }

        private void DrawToolSelection()
        {
            for (var i = 0; i < _tools.Count; i++)
            {
                GUI.backgroundColor = _selectedTool == i ? Color.gray : Color.white;
                if (GUILayout.Button(_tools[i].Name))
                {
                    _selectedTool = i;
                }
            }
        }
    }
}