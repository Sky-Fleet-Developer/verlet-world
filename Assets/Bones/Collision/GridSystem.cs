using Bones.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Bones.Collision
{
    public class GridSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter<NodeComponent, NodeVelocityComponent> _dynamicNodes;
        private EcsFilter<NodeComponent, NodeVelocityComponent, GridPositionComponent> _dynamicCellNodes;

        private readonly Grid _grid;

        public GridSystem(Grid grid)
        {
            _grid = grid;
        }
        
        public void Init()
        {
            foreach (int i in _dynamicNodes)
            {
                var entity = _dynamicNodes.GetEntity(i);
                entity.Replace(new GridPositionComponent());
            }
        }
        public void Run()
        {
            foreach (int i in _dynamicCellNodes)
            {
                ref NodeComponent node = ref _dynamicCellNodes.Get1(i);
                ref GridPositionComponent position = ref _dynamicCellNodes.Get3(i);
                position.Cell = _grid.WorldToCell(node.Position);
            }
        }
    }
}