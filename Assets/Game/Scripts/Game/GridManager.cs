using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class GridManager
    {
        public int cellsWidth { get; private set; }
        public int cellsHeight { get; private set; }
        public Vector2Int destinationCell { get; private set; }

        public HashSet<Vector2Int> blockedCells { get; private set; }

        private EcsWorld _world;

        public GridManager(EcsWorld world, int cellsWidth, int cellsHeight,
            Vector2Int destinationCell)
        {
            blockedCells = new HashSet<Vector2Int>();

            _world = world;
            this.cellsWidth = cellsWidth;
            this.cellsHeight = cellsHeight;
            this.destinationCell = destinationCell;
        }

        public void AddBlockCell(Vector2Int cell)
        {
            blockedCells.Add(cell);

            var eventEntity = _world.NewEntity();
            ref var pathUpdated = ref eventEntity.Get<PathUpdatedEvent>();
            pathUpdated.blockedCells = blockedCells;
            pathUpdated.width = cellsWidth;
            pathUpdated.height = cellsHeight;
        }

        public void RemoveBlockCell(Vector2Int cell)
        {
            blockedCells.Remove(cell);
        }
    }
}