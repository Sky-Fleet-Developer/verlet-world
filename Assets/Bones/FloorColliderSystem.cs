using Leopotam.Ecs;

namespace Bones
{
    public class FloorColliderSystem : IEcsRunSystem
    {
        private EcsFilter<NodeComponent> _nodesFilter;
        private EcsFilter<NodeVelocityComponent> _velocitiesFilter;

        public void Run()
        {
            foreach (int i in _velocitiesFilter)
            {
                ref NodeComponent node = ref _nodesFilter.Get1(i);
                ref NodeVelocityComponent velocity = ref _velocitiesFilter.Get1(i);

                if (node.Position.y < 0)
                {
                    node.Position.y = 0;
                    if (velocity.Velocity.y < 0)
                    {
                        velocity.Velocity.y *= -1f;
                        velocity.Velocity *= 0.8f;
                    }
                }
            }
        }
    }
}