using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public class StaticDataProvider
    {
        private readonly Dictionary<string, BuildingPoolData> _buildings = new ();
        private readonly Dictionary<string, MobPoolData> _mobs = new ();

        public void Init()
        {
            InitBuildings();
            InitMobs();
        }

        public BuildingPoolData GetBuildingData(string id)
        {
            if (_buildings.TryGetValue(id, out var b))
            {
                return b;
            }

            Debug.LogError($"Data with id {id} not found!");
            return null;
        }

        public MobPoolData GetMobData(string id)
        {
            if (_mobs.TryGetValue(id, out var m))
            {
                return m;
            }

            Debug.LogError($"Data with id {id} not found!");
            return null;
        }

        private void InitBuildings()
        {
            var buildings = Resources.LoadAll<BuildingPoolData>(
                "Data/Buildings");

            foreach (var b in buildings)
            {
                _buildings.Add(b.name, b);
            }
        }

        private void InitMobs()
        {
            var mobs = Resources.LoadAll<MobPoolData>(
                "Data/Mobs");

            foreach (var m in mobs)
            {
                _mobs.Add(m.name, m);
            }
        }
    }
}