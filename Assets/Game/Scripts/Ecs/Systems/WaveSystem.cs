using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class WaveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<WaveComponent> _filter;

        private readonly EcsWorld _world;
        private readonly StaticDataProvider _staticData;

        public WaveSystem(EcsWorld world, StaticDataProvider staticData)
        {
            _world = world;
            _staticData = staticData;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var wave = ref _filter.Get1(i);

                wave.spawnPeriodTimer += Time.deltaTime;

                if (wave.spawnPeriodTimer >= wave.spawnPeriod)
                {
                    wave.spawnPeriodTimer = 0f;

                    var requestEntity = _world.NewEntity();
                    ref var spawnRequest = ref requestEntity.Get<SpawnUnitRequest>();
                    spawnRequest.data = _staticData.GetMobData(wave.mobId);
                    spawnRequest.worlsPos = wave.spawnPos;

                    wave.spawnedValue += 1;

                    if (wave.spawnedValue >= wave.amount)
                    {
                        _filter.GetEntity(i).Destroy();
                    }
                }
            }
        }
    }
}