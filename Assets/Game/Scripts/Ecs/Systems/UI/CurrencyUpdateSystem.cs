using Leopotam.Ecs;

namespace TD
{
    public class CurrencyUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CurrencyChangedEvent> _filter;

        private readonly GameView _view;

        public CurrencyUpdateSystem(GameView view)
        {
            _view = view;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var changed = ref _filter.Get1(i);

                _view.SetCurrencyValue(changed.currency, changed.value);

                _filter.GetEntity(i).Del<CurrencyChangedEvent>();
            }
        }
    }
}