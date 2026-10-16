using Leopotam.Ecs;

namespace TD
{
    public class CurrencySystem : IEcsRunSystem
    {
        private readonly EcsFilter<ChangeCurrencyCommand> _filter;

        private EcsEntity _playerEntity;

        public CurrencySystem(EcsEntity playerEntity)
        {
            _playerEntity = playerEntity;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var command = ref _filter.Get1(i);
                ref var currency = ref _playerEntity.Get<PlayerCurrencyComponent>();

                switch (command.currency)
                {
                    case CurrencyType.Gold:
                        currency.gold += command.value;
                        break;

                    case CurrencyType.Water:
                        currency.water += command.value;
                        break;

                    case CurrencyType.Fire:
                        currency.fire += command.value;
                        break;

                    case CurrencyType.Earth:
                        currency.earth += command.value;
                        break;

                    case CurrencyType.Air:
                        currency.air += command.value;
                        break;
                }

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}