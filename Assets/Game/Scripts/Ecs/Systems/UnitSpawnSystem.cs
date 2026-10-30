using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class UnitSpawnSystem : IEcsRunSystem
    {
        private readonly EcsFilter<SpawnUnitRequest> _filter;

        private readonly EcsWorld _world;

        private GridManager _gridManager;

        public UnitSpawnSystem(EcsWorld world, GridManager gridManager)
        {
            _world = world;
            _gridManager = gridManager;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var request = ref _filter.Get1(i);
                var data = request.data;
                var unitEntity = _world.NewEntity();

                ref var position = ref unitEntity.Get<UnitPosition>();
                position.value = request.worlsPos;

                ref var movement = ref unitEntity.Get<UnitMovement>();
                var pos = new Vector2Int((int)position.value.x, (int)position.value.z);
                movement.path = GridPathfinder.FindPath(
                    pos,
                    _gridManager.destinationCell,
                    _gridManager.cellsWidth,
                    _gridManager.cellsHeight,
                    _gridManager.blockedCells
                    ).ToArray();

                movement.pathIndex = 0;
                movement.speed = data.movespeed;

                ref var state = ref unitEntity.Get<UnitStateComponent>();

                ref var hide = ref unitEntity.Get<UnitHideComponent>();

                unitEntity.Get<UnitHealthComponent>() = new UnitHealthComponent()
                {
                    baseHealthValue = data.baseHealthValue,
                    currentHealthValue = data.baseHealthValue
                };

                unitEntity.Get<RewardComponent>() = new RewardComponent()
                {
                    amount = data.rewards
                };

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}