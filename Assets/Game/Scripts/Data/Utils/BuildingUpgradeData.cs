using System;

namespace TD
{
    [Serializable]
    public class BuildingUpgradeData
    {
        public BuildingPoolData data;
        public CurrencyAmount[] costs;
    }
}
