using Leopotam.Ecs;

namespace TD
{
    public class CurrencySystem : IEcsRunSystem
    {
        private readonly EcsFilter<IncreaseCurrencyCommand> _increase;
        private readonly EcsFilter<DecreaseCurrencyCommand> _decrease;

        private readonly EcsWorld _world;
        private readonly EcsEntity _playerEntity;

        public CurrencySystem(EcsWorld world, EcsEntity playerEntity)
        {
            _world = world;
            _playerEntity = playerEntity;
        }

        public void Run()
        {
            foreach (var i in _increase)
            {
                ref var command = ref _increase.Get1(i);
                ref var currency = ref _playerEntity.Get<PlayerCurrencyComponent>();
                ref var entity = ref _increase.GetEntity(i);

                CurrencyUtility.AddCurrency(ref currency, command.amount);

                foreach (var c in command.amount)
                {
                    var currencyValue = CurrencyUtility.GetCurrencyValue(currency, c.currency);
                    var changedEntity = _world.NewEntity();
                    ref var changed = ref changedEntity.Get<CurrencyChangedEvent>();
                    changed.currency = c.currency;
                    changed.value = currencyValue;
                }

                _increase.GetEntity(i).Destroy();
            }

            foreach (var i in _decrease)
            {
                ref var command = ref _decrease.Get1(i);
                ref var currency = ref _playerEntity.Get<PlayerCurrencyComponent>();
                ref var entity = ref _decrease.GetEntity(i);

                bool spended = CurrencyUtility.TrySpend(ref currency, command.amount);
                if (spended)
                {
                    foreach (var c in command.amount)
                    {
                        var currencyValue = CurrencyUtility.GetCurrencyValue(currency, c.currency);
                        var changedEntity = _world.NewEntity();
                        ref var changed = ref changedEntity.Get<CurrencyChangedEvent>();
                        changed.currency = c.currency;
                        changed.value = currencyValue;
                    }
                }

                _decrease.GetEntity(i).Destroy();
            }
        }
    }
}