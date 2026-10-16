using Leopotam.Ecs;

namespace TD
{
    public struct OnTriggerExitEvent
    {
        public EcsEntity ownerEntity;
        public EcsEntity exitedEntity;
    }
}