using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class PathUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PathUpdatedEvent> _pathUpdate;
        private readonly EcsFilter<UnitMovement, UnitPosition> _units;

        private Vector2Int _destination;

        public PathUpdateSystem(Vector2Int destination)
        {
            _destination = destination;
        }

        public void Run()
        {
            foreach (var i in _pathUpdate)
            {
                var pathUpdate = _pathUpdate.Get1(i);

                foreach (var k in _units)
                {
                    ref var movement = ref _units.Get1(k);
                    ref var position = ref _units.Get2(k);
                    var pos = new Vector2Int((int)position.value.x, (int)position.value.z);

                    var updatedPath = GridPathfinder.FindPath(
                        pos,
                        _destination,
                        pathUpdate.width,
                        pathUpdate.height,
                        pathUpdate.blockedCells
                        ).ToArray();

                    if (updatedPath != null && updatedPath.Length > 0)
                    {
                        movement.path = updatedPath;
                        movement.pathIndex = 0;
                    }
                }

                _pathUpdate.GetEntity(i).Destroy();
            }
        }
    }
}