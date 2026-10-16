using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class TowerAttackSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TowerAttackComponent, TowerTargetComponent, UnitPosition> _filter;

        private EcsWorld _world;

        public TowerAttackSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var entity = ref _filter.GetEntity(i);
                ref var attack = ref _filter.Get1(i);
                ref var targetComp = ref _filter.Get2(i);

                if (attack.attackCooldownTimer > 0f)
                {
                    continue;
                }

                if (targetComp.target.Has<IsDeadTag>())
                {
                    entity.Del<TowerTargetComponent>();
                    continue;
                }

                attack.attackCooldownTimer = attack.attackCooldownDuration;

                var projectile = _world.NewEntity();

                projectile.Get<ProjectileComponent>() = new ProjectileComponent
                {
                    target = targetComp.target,
                    speed = 10f,
                    damageValue = 1,
                };

                projectile.Get<ProjectilePosition>() = new ProjectilePosition
                {
                    value = attack.muzzleTrans.position
                };
            }
        }
    }
}