using Leopotam.Ecs;

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
                var requestEntity = _world.NewEntity();

                switch (clickRequest.requestType)
                {
                    case UIRequestType.BuildingSelect:
                        ref var buildRequest = ref requestEntity.Get<BuildRequest>();
                        buildRequest.buildingId = clickRequest.id;
                        break;
                }

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}