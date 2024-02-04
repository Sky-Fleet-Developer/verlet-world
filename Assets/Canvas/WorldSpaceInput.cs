using System;
using UnityEngine;

namespace Canvas
{
    public class WorldSpaceInput : MonoBehaviour
    {
        private Plane _zeroPlane = new Plane(-Vector3.forward, 0);
        private Camera _mainCamera;
        private Vector3 _mousePosition;
        public Vector3 MousePosition => _mousePosition;
        public event Action<int> MouseDown;
        public event Action<int> MouseUp;
        private bool[] _mousePressed = new bool[3];
        public bool MousePressed(int idx) => _mousePressed[idx];
        
        void Start()
        {
            _mainCamera = Camera.main;
        }

        void Update()
        {
            Ray camRay = _mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            _zeroPlane.Raycast(camRay, out float d);
            _mousePosition = camRay.GetPoint(d);
            for (int i = 0; i < 2; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    MouseDown?.Invoke(i);
                }
                if (Input.GetMouseButtonUp(i))
                {
                    MouseUp?.Invoke(i);
                }
                _mousePressed[i] = Input.GetMouseButton(i);
            }
        }
    }
}
