using Leopotam.Ecs;

namespace TD
{
    public class BuildRequestSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildRequest> _filter;

        private readonly EcsWorld _world;
        private EcsEntity _playerEntity;
        private readonly StaticDataProvider _staticData;

        public BuildRequestSystem(EcsWorld world,
            EcsEntity playerEntity,
            StaticDataProvider staticData)
        {
            _world = world;
            _playerEntity = playerEntity;
            _staticData = staticData;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var buildRequest = ref _filter.Get1(i);
                var playerCurrency = _playerEntity.Get<PlayerCurrencyComponent>();
                var buildingData = _staticData.GetBuildingData(buildRequest.buildingId);

                if (CurrencyUtility.CanPurchase(playerCurrency,  buildingData.costs))
                {
                    var requestEntity = _world.NewEntity();
                    ref var request = ref requestEntity.Get<StartPlacementRequest>();
                    request.buildingId = buildRequest.buildingId;
                }
                else
                {
                    UnityEngine.Debug.Log("NOT ENOUGH GOLD!");
                }

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}