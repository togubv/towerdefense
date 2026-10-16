using Leopotam.Ecs;

namespace TD
{
    public class UnitSpawnSystem : IEcsRunSystem
    {
        private readonly EcsFilter<SpawnUnitRequest> _filter;

        private readonly EcsWorld _world;
        private readonly RouteStorage _routeStorage;

        public UnitSpawnSystem(EcsWorld world, RouteStorage routeStorage)
        {
            _world = world;
            _routeStorage = routeStorage;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var request = ref _filter.Get1(i);
                var data = request.data;
                var unitEntity = _world.NewEntity();

                ref var movement = ref unitEntity.Get<UnitMovement>();
                movement.routeId = 0;
                movement.routeStorage = _routeStorage;
                movement.pointIndex = 0;
                movement.speed = data.movespeed;

                ref var position = ref unitEntity.Get<UnitPosition>();
                position.value = request.worlsPos;

                ref var state = ref unitEntity.Get<UnitStateComponent>();
                //state.unitId = Random.Range(0, unitPrefabs.Length);

                ref var hide = ref unitEntity.Get<UnitHideComponent>();

                unitEntity.Get<UnitHealthComponent>() = new UnitHealthComponent()
                {
                    baseHealthValue = data.baseHealthValue,
                    currentHealthValue = data.baseHealthValue
                };

                unitEntity.Get<RewardComponent>() = new RewardComponent()
                {
                    gold = data.gold,
                    water = data.water,
                    fire = data.fire,
                    earth = data.earth,
                    air = data.air
                };

                _filter.GetEntity(i).Destroy();
            }
        }
    }
}