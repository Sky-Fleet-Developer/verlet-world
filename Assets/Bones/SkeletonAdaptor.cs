using System;
using System.Collections.Generic;
using Bones.Components;
using Bones.Systems;
using Canvas;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Bones
{
    public class SkeletonAdaptor : EscAdaptor
    {
        public int NodesCount { get; private set; }
        private DiContainer _diContainer;
        private int _iterationsCount = 30;
        [Inject] private TimeService _timeService;
        [Inject]
        private void Inject(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public override void Init()
        {
            NodesCount = 0;
            base.Init();
        }

        protected override IEnumerable<EcsSystems> Create(Func<EcsSystems> creator)
        {
            yield return creator();
            yield return creator().Add(new FlatVerletSystem(false)).Add(new FloorColliderSystem()).Add(new GravitySystem());
            yield return creator().Add(new HandlePointSystem()).Add(new DebugDrawEdgesSystem()).Add(new LeverInputSystem());
        }
        
        protected override EcsSystems Inject(EcsSystems systems)
        {
            foreach (IEcsSystem ecsSystem in systems.GetAllSystems().Items)
            {
                if (ecsSystem != null)
                {
                    _diContainer.Inject(ecsSystem);
                }
            }
            return systems.Inject(this);
        }

        public int CreateNode(Vector3 position)
        {
            EcsEntity entity = World.NewEntity();
            entity.Replace(new NodeComponent(position));
            return NodesCount++;
        }
        
        /*public void CreateEdge(int nodeA, int nodeB)
        {
            EcsEntity entity = World.NewEntity();
            entity.Replace(new EdgeComponent {NodeA = nodeA, NodeB = nodeB });
        }*/

        
        void FixedUpdate()
        {
            if (!IsInitialized) return;
            for (int i = 0; i < _timeService.Iterations; i++)
            {
                GetSystemsGroup(1).Run();
            }
        }

        void Update()
        {
            if (!IsInitialized) return;
            GetSystemsGroup(2).Run();
        }
    }
}
