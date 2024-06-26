﻿using Bones.Components;
using Canvas;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Bones.Systems
{
    public class FlatVerletSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter<NodeComponent> _nodesFilter; 
        private EcsFilter<EdgeComponent> _edgesFilter; 
        private EcsFilter<NodeVelocityComponent> _velocitiesFilter;
        [Inject] private TimeService _timeService;
        private EcsWorld _world;
        private bool _needInitEdgesLength;

        public FlatVerletSystem(bool needInitEdgesLength)
        {
            _needInitEdgesLength = needInitEdgesLength;
        }
        public void Init()
        {
            foreach (int i in _edgesFilter)
            {
                ref EdgeComponent edgeComponent = ref _edgesFilter.Get1(i);
                NodeComponent nodeA = _nodesFilter.Get1(edgeComponent.NodeA);
                NodeComponent nodeB = _nodesFilter.Get1(edgeComponent.NodeB);
                _nodesFilter.GetEntity(edgeComponent.NodeA).Replace(new NodeVelocityComponent());
                _nodesFilter.GetEntity(edgeComponent.NodeB).Replace(new NodeVelocityComponent());
                if (_needInitEdgesLength)
                {
                    edgeComponent.Length = Vector3.Distance(nodeA.Position, new Vector3(nodeB.Position.x, nodeB.Position.y, nodeA.Position.z));
                }
            }
        }
        
        public void Run()
        {
            foreach (int i in _edgesFilter)
            {
                EdgeComponent edgeComponent = _edgesFilter.Get1(i);
                ref NodeComponent nodeA = ref _nodesFilter.Get1(edgeComponent.NodeA);
                ref NodeVelocityComponent velocityA = ref _velocitiesFilter.Get1(edgeComponent.NodeA);
                ref NodeComponent nodeB = ref _nodesFilter.Get1(edgeComponent.NodeB);
                ref NodeVelocityComponent velocityB = ref _velocitiesFilter.Get1(edgeComponent.NodeB);
                float edgeLength = edgeComponent.Length;

                Vector3 deltaAb = nodeB.Position - nodeA.Position;
                deltaAb.z = 0;
                float currentLength = deltaAb.magnitude;
                Vector3 directionAb = deltaAb / currentLength;

                float pushDistance = edgeLength - currentLength;
                Vector3 springImpulse = directionAb * (pushDistance * _timeService.DeltaTime * 0.5f) * edgeComponent.Spring;

                Vector3 dumperImpulse = directionAb * Vector3.Dot(velocityB.Velocity - velocityA.Velocity, directionAb) * edgeComponent.Dumper * _timeService.DeltaTime;
                if (!nodeA.IsStatic)
                {
                    velocityA.Velocity -= springImpulse;
                    velocityA.Velocity += dumperImpulse;
                }

                if (!nodeB.IsStatic)
                {
                    velocityB.Velocity += springImpulse;
                    velocityB.Velocity -= dumperImpulse;
                }
            }

            foreach (int i in _nodesFilter)
            {
                ref NodeComponent node = ref _nodesFilter.Get1(i);
                if (!node.IsStatic)
                {
                    ref NodeVelocityComponent velocity = ref _velocitiesFilter.Get1(i);
                    node.Position += velocity.Velocity * _timeService.DeltaTime;
                }
            }
        }
    }
}