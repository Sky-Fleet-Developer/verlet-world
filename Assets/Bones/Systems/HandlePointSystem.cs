using Bones.Components;
using Canvas;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Bones.Systems
{
    public class HandlePointSystem : IEcsRunSystem, IEcsInitSystem
    {
        [Inject] private WorldSpaceInput _worldSpaceInput;
        private EcsFilter<NodeComponent> _nodesFilter;
        private EcsFilter<NodeVelocityComponent> _velocitiesFilter;
        private EscAdaptor _adaptor;
        private int? _selectedNode;
        private float _sqrSelectDistance = 0.1f;
        
        public void Init()
        {
            _worldSpaceInput.MouseDown += OnMouseDown;
            _worldSpaceInput.MouseUp += OnMouseUp;
            _adaptor.WorldDestroying += OnWorldDestroying;
        }

        private void OnWorldDestroying()
        {
            _worldSpaceInput.MouseDown -= OnMouseDown;
            _worldSpaceInput.MouseUp -= OnMouseUp;
            _adaptor.WorldDestroying -= OnWorldDestroying;
        }

        private void OnMouseDown(int key)
        {
            float distance = _sqrSelectDistance;
            foreach (int i in _nodesFilter)
            {
                ref NodeComponent node = ref _nodesFilter.Get1(i);
                float tempDist = Vector3.SqrMagnitude(node.Position - _worldSpaceInput.MousePosition);
                if (tempDist < distance)
                {
                    _selectedNode = i;
                }
            }
        }
        
        private void OnMouseUp(int key)
        {
            if (_selectedNode.HasValue)
            {
                ref NodeComponent node = ref _nodesFilter.Get1(_selectedNode.Value);
                node.IsStatic = false;
                _selectedNode = null;
            }
        }

        public void Run()
        {
            if (_selectedNode.HasValue)
            {
                ref NodeComponent node = ref _nodesFilter.Get1(_selectedNode.Value);
                node.Position = _worldSpaceInput.MousePosition;
                node.IsStatic = true;
                ref NodeVelocityComponent velocity = ref _velocitiesFilter.Get1(_selectedNode.Value);
                velocity.Velocity = Vector3.zero;
            }
        }
    }
}