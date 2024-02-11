using Bones.Components;
using Canvas;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Bones.Systems
{
    public class GravitySystem : IEcsRunSystem
    {
        private EcsFilter<NodeVelocityComponent> _velocitiesFilter;
        [Inject] private TimeService _timeService;
        private float _gravity = 9.8f;

        public void Run()
        {
            foreach (int i in _velocitiesFilter)
            {
                ref NodeVelocityComponent velocity = ref _velocitiesFilter.Get1(i);
                velocity.Velocity += Vector3.down * (_timeService.DeltaTime * _gravity);
            }
        }
    }
}