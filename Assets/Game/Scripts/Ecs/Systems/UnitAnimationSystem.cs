using Leopotam.Ecs;

namespace TD
{
    public class UnitAnimationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitStateComponent,
            UnitViewComponent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var state = ref _filter.Get1(i);
                ref var view = ref _filter.Get2(i);

                if (state.current == state.previous)
                {
                    continue;
                }

                view.view.animator.SetInteger("state", (int)state.current);

                state.previous = state.current;
            }
        }
    
    }
}
