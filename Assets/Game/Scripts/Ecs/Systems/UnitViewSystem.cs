using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class UnitViewSystem : IEcsRunSystem
    {
        private readonly UnitView[] _prefabs;
        private readonly Transform _parent;

        private EcsFilter<UnitPosition, UnitStateComponent>.Exclude<UnitViewComponent> _filter;

        public UnitViewSystem(UnitView[] prefabs, Transform parent)
        {
            _prefabs = prefabs;
            _parent = parent;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);
                ref var state = ref _filter.Get2(i);
                var entity = _filter.GetEntity(i);

                UnitView view = Object.Instantiate(_prefabs[state.unitId], position.value,
                    Quaternion.identity, _parent);

                view.entity = entity;
                entity.Get<UnitViewComponent>().trans = view.transform;
                entity.Get<UnitViewComponent>().view = view;
            }
        }
    }
}
