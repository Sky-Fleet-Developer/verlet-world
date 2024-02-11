using UnityEngine;

namespace GridEditor
{
    public class IntFieldGui
    {
        private int _value;
        private string _inputFieldText;
        private string _label;
        public int Value => _value;

        public IntFieldGui(string label, int initialValue)
        {
            _label = label;
            _value = initialValue;
            _inputFieldText = _value.ToString();
        }

        public void DrawGUIElement()
        {
            GUI.SetNextControlName(_label);
            GUILayout.BeginHorizontal();
            GUILayout.Label(_label);
            _inputFieldText = GUILayout.TextField(_inputFieldText);
            GUILayout.EndHorizontal();
            if (Event.current.type == EventType.Repaint)
            {
                if (int.TryParse(_inputFieldText, out int newValue))
                {
                    _value = newValue;
                }
                
                    //_inputFieldText = _value.ToString();
            }
        }
    }
}