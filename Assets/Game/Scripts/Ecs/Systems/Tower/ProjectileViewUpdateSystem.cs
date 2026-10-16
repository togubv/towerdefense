using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class ProjectileViewUpdateSystem : IEcsRunSystem
    {
        private EcsFilter<ProjectileComponent, ProjectilePosition, ProjectileViewComponent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var projectile = ref _filter.Get1(i);
                ref var position = ref _filter.Get2(i);
                ref var view = ref _filter.Get3(i);

                if (position.rotation != Vector3.zero)
                {
                    view.trans.rotation = Quaternion.LookRotation(position.rotation);
                }

                view.trans.position = position.value;
            }
        }
    }
}