using System;
using System.Collections.Generic;
using Bones.Collision;
using Bones.Components;
using Bones.Systems;
using Canvas;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Tilemaps;
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
            yield return creator().Add(new FlatVerletSystem(false)).Add(new GravitySystem());//.Add(new FloorColliderSystem())
            var grid = _diContainer.Resolve<Grid>();
            var tilemap = _diContainer.Resolve<Tilemap>();
            yield return creator().Add(new GridSystem(grid)).Add(new CollideSystem(tilemap, grid, 0.2f)).Add(new DebugDrawEdgesSystem()).Add(new DebugVelocitiesDrawSystem());
            yield return creator().Add(new HandlePointSystem()).Add(new LeverInputSystem());
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
            GetSystemsGroup(2).Run();
        }

        void Update()
        {
            if (!IsInitialized) return;
            GetSystemsGroup(3).Run();
        }
    }
}
