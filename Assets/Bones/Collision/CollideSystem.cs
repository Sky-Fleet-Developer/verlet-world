using System.Collections.Generic;
using Bones.Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bones.Collision
{
    public class CollideSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter<NodeComponent, NodeVelocityComponent, GridPositionComponent> _dynamicCellNodes;
        private readonly Dictionary<Vector3Int, Vector3> _filledCells = new Dictionary<Vector3Int, Vector3>();
        private readonly Tilemap _tilemap;
        private readonly Grid _grid;
        private float _collisionThreshold;
        private Vector2 _cellSize;
        private Vector2 _cellHalfSize;
        private Vector2 _cellHalfSizeWithThreshold;
        public CollideSystem(Tilemap tilemap, Grid grid, float collisionThreshold)
        {
            _tilemap = tilemap;
            _grid = grid;
            _collisionThreshold = collisionThreshold;
            _cellSize = _grid.cellSize;
            _cellHalfSize = _cellSize * 0.5f;
            _cellHalfSizeWithThreshold = _cellHalfSize + Vector2.one * _collisionThreshold;
        }
        
        public void Init()
        {
            var cellBounds = _tilemap.cellBounds;
            for (int x = cellBounds.xMin; x <= cellBounds.xMax; x++)
            {
                for (int y = cellBounds.yMin; y <= cellBounds.yMax; y++)
                {
                    var pos = new Vector3Int(x, y, 0);
                    if (_tilemap.HasTile(pos))
                    {
                        _filledCells.Add(pos, _grid.CellToWorld(pos) + (Vector3)_cellHalfSize);
                    }
                }
            }   
        }

        private Vector3Int[] neighbours = new[]
        {
            new Vector3Int(-1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0),
        };
        public void Run()
        {
            foreach (int i in _dynamicCellNodes)
            {
                ref NodeComponent node = ref _dynamicCellNodes.Get1(i);
                ref NodeVelocityComponent velocity = ref _dynamicCellNodes.Get2(i);
                GridPositionComponent position = _dynamicCellNodes.Get3(i);
                Vector3 acceleration = Vector3.zero;
                Vector3 displacement = Vector3.zero;
                Vector3Int cell = position.Cell;
                foreach (Vector3Int vector3Int in neighbours)
                {
                    CollideBox(cell + vector3Int, node.Position, velocity.Velocity, ref displacement, ref acceleration);
                }

                node.Position += displacement;
                velocity.Velocity += acceleration;
            }
        }

        private void CollideBox(Vector3Int cell, Vector3 position, Vector3 velocity, ref Vector3 displacement, ref Vector3 acceleration)
        {
            if (_filledCells.TryGetValue(cell, out Vector3 gridCellCenter))
            {
                Vector3 outOfCenter = position - gridCellCenter;
                float absX = Mathf.Abs(outOfCenter.x);
                float absY = Mathf.Abs(outOfCenter.y);
                if (absX > _cellHalfSizeWithThreshold.x || absY > _cellHalfSizeWithThreshold.y)
                {
                    return;
                }                
                bool isMovingToCenter = Vector3.Dot(outOfCenter, velocity) < 0;
                Debug.DrawLine(position, gridCellCenter, Color.red);
                Vector3 pushDirection;
                if (absX > absY)
                {
                    pushDirection = Vector2.right * outOfCenter.x;
                    if (pushDirection.x > 0)
                    {
                        pushDirection.x -= _cellHalfSizeWithThreshold.x;
                    }
                    else
                    {
                        pushDirection.x += _cellHalfSizeWithThreshold.x;
                    }

                    if (isMovingToCenter)
                    {
                        acceleration = Vector3.right * velocity.x * -1f;
                    }
                }
                else
                {
                    pushDirection = Vector2.up * outOfCenter.y;
                    if (pushDirection.y > 0)
                    {
                        pushDirection.y -= _cellHalfSizeWithThreshold.y;
                    }
                    else
                    {
                        pushDirection.y += _cellHalfSizeWithThreshold.y;
                    }

                    if (isMovingToCenter)
                    {
                        acceleration = Vector3.up * velocity.y * -1f;
                    }
                }

                pushDirection *= -1;
                Debug.DrawRay(position, pushDirection, Color.yellow);
                displacement += pushDirection;
            }
        }
    }
}