using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class TowerAttackCooldownSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TowerAttackComponent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var attack = ref _filter.Get1(i);

                if (attack.attackCooldownTimer == 0f)
                {
                    continue;
                }

                attack.attackCooldownTimer -= Time.deltaTime;

                if (attack.attackCooldownTimer <= 0f)
                {
                    attack.attackCooldownTimer = 0f;
                }
            }
        }
    }
}