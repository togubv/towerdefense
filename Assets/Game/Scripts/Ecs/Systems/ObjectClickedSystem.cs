using Leopotam.Ecs;

namespace TD
{
    public class ObjectClickedSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ObjectClickedEvent> _filter;
        private readonly EcsFilter<BuildingComponent, BuildingViewComponent> _buildings;

        private UpgradeUIView _upgradeView;
        private StaticDataProvider _staticData;
        private EcsEntity _playerEntity;

        public ObjectClickedSystem(UpgradeUIView upgradeView,
            StaticDataProvider staticData, EcsEntity playerEntity)
        {
            _upgradeView = upgradeView;
            _staticData = staticData;
            _playerEntity = playerEntity;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                var objectClick = _filter.Get1(i);

                if (objectClick.view == null)
                {
                    _upgradeView.HideUpgrades();
                }
                else
                {
                    foreach (var k in _buildings)
                    {
                        var view = _buildings.Get2(k);

                        if (objectClick.view != view.view)
                        {
                            continue;
                        }

                        ref var building = ref _buildings.Get1(k);
                        var buildingData = _staticData.GetBuildingData(building.buildingId);
                        _upgradeView.ShowUpgrades(objectClick.clickWorldPos, buildingData, view.entity);
                        _upgradeView.UpdateBuildingsAvailables(_playerEntity.Get<PlayerCurrencyComponent>().amount);
                        break;
                    }
                }

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}