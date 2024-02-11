using UnityEngine;

namespace GridEditor
{
    public class FloatFieldGui
    {
        private float _value;
        private string _inputFieldText;
        private string _prevInputFieldText;
        private bool _hasFocus;
        private string _label;
        public float Value => _value;

        public FloatFieldGui(string label, float initialValue)
        {
            _label = label;
            _value = initialValue;
            _inputFieldText = _value.ToString();
            _prevInputFieldText = _inputFieldText;
            _hasFocus = false;
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
                if (_hasFocus)
                {
                    GUI.FocusControl(_label);
                }

                if (!_inputFieldText.Equals(_prevInputFieldText))
                {
                    _hasFocus = true;
                }
                else
                {
                    _hasFocus = false;
                }

                if (!_hasFocus)
                {
                    if (float.TryParse(_inputFieldText, out float newValue))
                    {
                        _value = newValue;
                        _prevInputFieldText = _inputFieldText;
                    }
                }
            }
        }
    }
}