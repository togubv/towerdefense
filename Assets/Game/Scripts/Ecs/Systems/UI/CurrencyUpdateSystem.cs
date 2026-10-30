using Leopotam.Ecs;

namespace TD
{
    public class CurrencyUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CurrencyChangedEvent> _filter;

        private readonly GameUIView _view;
        private readonly BuildUIView _buildView;
        private readonly UpgradeUIView _upgradeView;
        private readonly EcsEntity _playerEntity;

        public CurrencyUpdateSystem(EcsEntity playerEntity,
            GameUIView view,
            BuildUIView buildView,
            UpgradeUIView upgradeView)
        {
            _playerEntity = playerEntity;
            _view = view;
            _buildView = buildView;
            _upgradeView = upgradeView;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var changed = ref _filter.Get1(i);
                var playerCurrency = _playerEntity.Get<PlayerCurrencyComponent>();

                _view.SetCurrencyValue(changed.currency, changed.value);
                _buildView.UpdateBuildingsAvailables(playerCurrency.amount);
                _upgradeView.UpdateBuildingsAvailables(playerCurrency.amount);

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}