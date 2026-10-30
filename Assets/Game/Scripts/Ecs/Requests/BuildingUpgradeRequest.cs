using Leopotam.Ecs;

namespace TD
{
    public struct BuildingUpgradeRequest
    {
        public EcsEntity buildingEntity;
        public BuildingUpgradeData upgradeData;
    }
}