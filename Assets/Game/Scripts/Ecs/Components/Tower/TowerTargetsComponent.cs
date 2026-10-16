using System.Collections.Generic;
using Leopotam.Ecs;

namespace TD
{
    public struct TowerTargetsComponent
    {
        public HashSet<EcsEntity> targetsInRadius;
    }
}