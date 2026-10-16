using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class InputProcessingUISystem : IEcsRunSystem
    {
        private readonly EcsFilter<ClickUIRequest> _filter;

        private readonly EcsWorld _world;

        public InputProcessingUISystem(EcsWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var clickRequest = ref _filter.Get1(i);

                switch (clickRequest.requestType)
                {
                    case UIRequestType.BuildingSelect:
                        var requestEntity = _world.NewEntity();
                        ref var request = ref requestEntity.Get<StartPlacementRequest>();
                        request.buildingId = clickRequest.id;
                        break;
                }

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}