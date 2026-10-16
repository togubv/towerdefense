using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class PlacementInitSystem : IEcsRunSystem
    {
        private readonly EcsFilter<StartPlacementRequest> _requests;
        private readonly EcsFilter<PlacementPreviewComponent> _activePreviews;

        private readonly EcsWorld _world;
        private readonly StaticDataProvider _staticData;

        public PlacementInitSystem(EcsWorld world, StaticDataProvider staticData)
        {
            _world = world;
            _staticData = staticData;
        }

        public void Run()
        {
            foreach (var i in _requests)
            {
                ref var request = ref _requests.Get1(i);

                foreach (var p in _activePreviews)
                {
                    Object.Destroy(_activePreviews.Get1(p).previewTrans.gameObject);
                    _activePreviews.GetEntity(p).Destroy();
                }

                var buildingData = _staticData.GetBuildingData(request.buildingId);

                if (buildingData != null && buildingData.prefab != null)
                {
                    var previewView = Object.Instantiate(buildingData.prefab);
                    previewView.SetPreviewState();
                    var previewEntity = _world.NewEntity();
                    ref var preview = ref previewEntity.Get<PlacementPreviewComponent>();
                    preview.data = buildingData;
                    preview.previewTrans = previewView.transform;
                }

                _requests.GetEntity(i).Destroy();
            }
        }
    }
}