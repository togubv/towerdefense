using Leopotam.Ecs;

namespace TD
{
    public class CurrencySystem : IEcsRunSystem
    {
        private readonly EcsFilter<ChangeCurrencyCommand> _filter;
        private readonly EcsFilter<AddRewardsCommand> _rewards;

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
                    case CurrencyType.Lives:
                        currency.lives += command.value;
                        break;

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

                _filter.GetEntity(i).Del<PlayerCurrencyComponent>();
            }

            foreach (var i in _rewards)
            {
                ref var rewards = ref _rewards.Get1(i);
                ref var currency = ref _playerEntity.Get<PlayerCurrencyComponent>();

                if (rewards.lives > 0)
                {
                    currency.lives += rewards.lives;
                    _rewards.GetEntity(i).Get<CurrencyChangedEvent>() =
                        new CurrencyChangedEvent(CurrencyType.Lives, currency.lives);
                }

                if (rewards.gold > 0)
                {
                    currency.gold += rewards.gold;
                    _rewards.GetEntity(i).Get<CurrencyChangedEvent>() =
                        new CurrencyChangedEvent(CurrencyType.Gold, currency.gold);
                }

                if (rewards.water > 0)
                {
                    currency.water += rewards.water;
                    _rewards.GetEntity(i).Get<CurrencyChangedEvent>() =
                        new CurrencyChangedEvent(CurrencyType.Water, currency.water);
                }

                if (rewards.fire > 0)
                {
                    currency.fire += rewards.fire;
                    _rewards.GetEntity(i).Get<CurrencyChangedEvent>() =
                        new CurrencyChangedEvent(CurrencyType.Fire, currency.fire);
                }

                if (rewards.earth > 0)
                {
                    currency.earth += rewards.earth;
                    _rewards.GetEntity(i).Get<CurrencyChangedEvent>() =
                        new CurrencyChangedEvent(CurrencyType.Earth, currency.earth);
                }

                if (rewards.air > 0)
                {
                    currency.air += rewards.air;
                    _rewards.GetEntity(i).Get<CurrencyChangedEvent>() =
                        new CurrencyChangedEvent(CurrencyType.Air, currency.air);
                }

                _rewards.GetEntity(i).Del<AddRewardsCommand>();
            }
        }
    }
}