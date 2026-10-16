using Leopotam.Ecs;

namespace TD
{
    public struct ProjectileComponent
    {
        public EcsEntity target;
        public float speed;
        public int damageValue;
    }
}