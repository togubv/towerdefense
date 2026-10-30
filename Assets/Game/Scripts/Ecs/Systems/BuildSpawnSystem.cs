using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class BuildSpawnSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildStructureRequest> _filter;

        private readonly EcsWorld _world;
        private readonly GridManager _gridManager;

        public BuildSpawnSystem(EcsWorld world, GridManager gridManager)
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

                var buildingEntity = _world.NewEntity();

                _gridManager.AddBlockCell(new Vector2Int((int)request.worlsPos.x, (int)request.worlsPos.z));

                buildingEntity.Get<BuildingComponent>() = new BuildingComponent
                {
                    buildingId = data.id
                };

                buildingEntity.Get<TowerTargetsComponent>().targetsInRadius =
                    new HashSet<EcsEntity>();

                buildingEntity.Get<TowerAttackComponent>() = new TowerAttackComponent
                {
                    attackCooldownDuration = data.attackPerSeconds,
                    attackCooldownTimer = 0f
                };

                buildingEntity.Get<UnitPosition>() = new UnitPosition()
                {
                    value = request.worlsPos
                };

                buildingEntity.Get<BuildingViewWaiter>() = new BuildingViewWaiter()
                {
                    poolData = data,
                    spawnPos = request.worlsPos
                };

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}