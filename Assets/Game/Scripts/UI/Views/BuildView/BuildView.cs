using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class BuildView : MonoBehaviour
    {
        public BuildingSlotContainer slotContainerPrefab;
        public Transform slotsParent;

        private List<BuildingSlotContainer> _buildingSlots;

        private EcsWorld _world;

        public void Init(EcsWorld world)
        {
            _world = world;
            _buildingSlots = new List<BuildingSlotContainer>();
        }

        public void InitSlots(BuildingPoolData[] buildings)
        {
            foreach (var b in buildings)
            {
                var spawnedSlot = Instantiate(slotContainerPrefab, slotsParent);
                spawnedSlot.Init(b.id, b.title, b.icon);
                _buildingSlots.Add(spawnedSlot);

                spawnedSlot.EventClick += BuildingSlotClickHandler;
            }
        }

        public void OnDestroy()
        {
            foreach (var b in _buildingSlots)
            {
                b.EventClick -= BuildingSlotClickHandler;
            }
        }

        private void BuildingSlotClickHandler(string buildingId)
        {
            var requestEntity = _world.NewEntity();
            ref var request = ref requestEntity.Get<ClickUIRequest>();
            request.requestType = UIRequestType.BuildingSelect;
            request.id = buildingId;
        }
    }
}
