using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class HealthSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitHealthComponent, DamageEvent>.Exclude<IsDeadTag> _filter;

        private EcsWorld _world;

        public HealthSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var health = ref _filter.Get1(i);
                ref var damage = ref _filter.Get2(i);

                health.currentHealthValue = Mathf.Clamp(
                    health.currentHealthValue - damage.value,
                    0,
                    health.baseHealthValue
                    );

                var entity = _filter.GetEntity(i);
                entity.Get<HealthChangedEvent>() = new HealthChangedEvent
                {
                    currentValue = health.currentHealthValue,
                    maxValue = health.baseHealthValue
                };

                entity.Del<DamageEvent>();

                if (health.currentHealthValue <= 0)
                {
                    ref var deadTag = ref entity.Get<IsDeadTag>();
                    deadTag.withAnimation = true;

                    if (entity.Has<RewardComponent>())
                    {
                        var reward = entity.Get<RewardComponent>();
                        var commandEntity = _world.NewEntity();
                        commandEntity.Get<IncreaseCurrencyCommand>() = new IncreaseCurrencyCommand
                        {
                            amount = reward.amount
                        };
                    }
                }
            }
        }
    }
}