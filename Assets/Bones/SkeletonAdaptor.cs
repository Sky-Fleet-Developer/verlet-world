using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Bones
{
    public class SkeletonAdaptor : EscAdaptor
    {
        public int NodesCount { get; private set; }
        private DiContainer _diContainer;
        
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
            yield return creator().Add(new FlatVerletSystem()).Add(new FloorColliderSystem()).Add(new GravitySystem());
            yield return creator().Add(new HandlePointSystem()).Add(new DebugDrawEdgesSystem());//.Add(new DebugDrawEdgesSystem());
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
            for (int i = 0; i < 10; i++)
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
