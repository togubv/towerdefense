using Leopotam.Ecs;

namespace TD
{
    public struct OnTriggerEnterEvent
    {
        public EcsEntity ownerEntity;
        public EcsEntity enteredEntity;
    }
}