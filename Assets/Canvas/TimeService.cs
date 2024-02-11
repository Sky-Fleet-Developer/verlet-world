using System;
using UnityEngine;

namespace Canvas
{
    public class TimeService : MonoBehaviour
    {
        public int Iterations => iterations;
        public float DeltaTime => _deltaTime;
        [SerializeField] private int iterations = 30;
        private float _deltaTime;

        private void OnValidate()
        {
            if (iterations <= 0)
            {
                iterations = 1;
            }
            _deltaTime = 0.01666667F / iterations;
        }
    }
}