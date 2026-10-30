using Leopotam.Ecs;

namespace TD
{
    public class HealthBarUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthChangedEvent, UnitViewComponent> _filter;

        private readonly BarsUIView _view;

        public HealthBarUpdateSystem(BarsUIView view)
        {
            _view = view;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var changed = ref _filter.Get1(i);
                ref var unitView = ref _filter.Get2(i);

                _view.ShowHealthBar(unitView.trans, changed.currentValue / changed.maxValue);
                _filter.GetEntity(i).Del<HealthChangedEvent>();
            }
        }
    }
}