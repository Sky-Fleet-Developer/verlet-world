using Bones.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Bones.Systems
{
    public class DebugVelocitiesDrawSystem : IEcsRunSystem
    {
        private EcsFilter<NodeComponent, NodeVelocityComponent> _nodesFilter;
        public void Run()
        {
            foreach (int i in _nodesFilter)
            {
                NodeComponent node = _nodesFilter.Get1(i);
                NodeVelocityComponent velocity = _nodesFilter.Get2(i);
                Debug.DrawRay(node.Position, velocity.Velocity, Color.green);
            }
        }
    }
}