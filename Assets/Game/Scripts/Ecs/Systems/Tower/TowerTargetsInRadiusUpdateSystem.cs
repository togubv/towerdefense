using Leopotam.Ecs;

namespace TD
{
    public class TowerTargetsInRadiusUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<OnTriggerEnterEvent> _enterEvents;
        private readonly EcsFilter<OnTriggerExitEvent> _exitEvents;

        public void Run()
        {
            foreach (var i in _enterEvents)
            {
                ref var enter = ref _enterEvents.Get1(i);
                var eventEntity = _enterEvents.GetEntity(i);

                if (enter.ownerEntity.IsAlive() == false ||
                    enter.enteredEntity.IsAlive() == false)
                {
                    eventEntity.Destroy();
                    continue;
                }

                enter.ownerEntity.Get<TowerTargetsComponent>()
                        .targetsInRadius.Add(enter.enteredEntity);

                eventEntity.Destroy();
            }

            foreach (var i in _exitEvents)
            {
                ref var exit = ref _exitEvents.Get1(i);
                var eventEntity = _exitEvents.GetEntity(i);

                if (exit.ownerEntity.IsAlive())
                {
                    if (exit.ownerEntity.Has<TowerTargetsComponent>())
                    {
                        var targets = exit.ownerEntity.Get<TowerTargetsComponent>()
                            .targetsInRadius.Remove(exit.exitedEntity);

                        if (exit.ownerEntity.Has<TowerTargetComponent>() &&
                            exit.ownerEntity.Get<TowerTargetComponent>().target == exit.exitedEntity)
                        {
                            exit.ownerEntity.Del<TowerTargetComponent>();
                        }
                    }
                }

                eventEntity.Destroy();
            }
        }
    }
}