using Leopotam.Ecs;
using UnityEngine;

namespace Bones
{
    public class GravitySystem : IEcsRunSystem
    {
        private EcsFilter<NodeVelocityComponent> _velocitiesFilter;

        public void Run()
        {
            foreach (int i in _velocitiesFilter)
            {
                ref NodeVelocityComponent velocity = ref _velocitiesFilter.Get1(i);
                velocity.Velocity += Vector3.down * Time.deltaTime * 0.3f;
            }
        }
    }
}