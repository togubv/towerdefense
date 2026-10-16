using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class ProjectileMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ProjectileComponent, ProjectilePosition> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var projectile = ref _filter.Get1(i);
                ref var position = ref _filter.Get2(i);

                if (projectile.target.IsAlive() == false)
                {
                    _filter.GetEntity(i).Destroy();
                    continue;
                }

                Vector3 targetPosition = projectile.target.Get<UnitPosition>().value;

                var distance = (targetPosition - position.value).sqrMagnitude;

                if (distance < 0.1f * 0.1f)
                {
                    projectile.target.Get<DamageEvent>() = new DamageEvent
                    {
                        value = 1f
                    };

                    var entity = _filter.GetEntity(i);

                    if (entity.Has<ProjectileViewComponent>())
                    {
                        var view = entity.Get<ProjectileViewComponent>();

                        if (view.view != null)
                        {
                            Object.Destroy(view.view.gameObject);
                        }
                    }

                    _filter.GetEntity(i).Destroy();
                    continue;
                }
                
                position.value = Vector3.MoveTowards(
                    position.value,
                    targetPosition,
                    projectile.speed * Time.deltaTime
                );
            }
        }
    }
}