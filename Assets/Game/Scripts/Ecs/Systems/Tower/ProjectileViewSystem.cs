using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class ProjectileViewSystem : IEcsRunSystem
    {
        private ProjectileView _prefab;
        private readonly Transform _parent;

        private EcsFilter<ProjectilePosition, ProjectileComponent>
            .Exclude<ProjectileViewComponent> _filter;

        public ProjectileViewSystem(ProjectileView prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);
                var entity = _filter.GetEntity(i);

                ProjectileView view = Object.Instantiate(_prefab, position.value,
                    Quaternion.identity, _parent);

                entity.Get<ProjectileViewComponent>().trans = view.transform;
                entity.Get<ProjectileViewComponent>().view = view;
            }
        }
    }
}