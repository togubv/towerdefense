using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class BuildingUpgradeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingUpgradeRequest> _filter;

        private readonly EcsWorld _world;
        private EcsEntity _playerEntity;


        public BuildingUpgradeSystem(EcsWorld world, EcsEntity playerEntity)
        {
            _world = world;
            _playerEntity = playerEntity;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {                
                ref var upgradeRequest = ref _filter.Get1(i);
                var buildingEntity = upgradeRequest.buildingEntity;

                var cost = upgradeRequest.upgradeData.costs;
                ref var currency = ref _playerEntity.Get<PlayerCurrencyComponent>();

                bool canPurchase = CurrencyUtility.CanPurchase(currency, cost);

                if (canPurchase)
                {
                    var changeCurrencyEntity = _world.NewEntity();
                    ref var changeCurrency = ref changeCurrencyEntity.Get<DecreaseCurrencyCommand>();
                    changeCurrency.amount = cost;

                    buildingEntity.Get<BuildingComponent>() = new BuildingComponent
                    {
                        buildingId = upgradeRequest.upgradeData.data.id
                    };

                    ref var attack = ref buildingEntity.Get<TowerAttackComponent>();
                    attack.attackCooldownDuration = upgradeRequest.upgradeData.data.attackPerSeconds;

                    Object.Destroy(buildingEntity.Get<BuildingViewComponent>().view.gameObject);
                    buildingEntity.Del<BuildingViewComponent>();

                    var spawnPos = buildingEntity.Get<UnitPosition>().value;

                    buildingEntity.Get<BuildingViewWaiter>() = new BuildingViewWaiter()
                    {
                        poolData = upgradeRequest.upgradeData.data,
                        spawnPos = spawnPos
                    };

                    var hideUpgradeContainerEntity = _world.NewEntity();
                    ref var nullEvent = ref hideUpgradeContainerEntity.Get<ObjectClickedEvent>();
                    nullEvent.view = null;
                }

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}