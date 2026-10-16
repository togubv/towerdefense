using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;

#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;
#endif

namespace TD
{
    public class EcsLoader : MonoBehaviour
    {
        public UnitView[] unitPrefabs;
        public ProjectileView projectilePrefab;
        public Transform unitsParent;
        public Transform buildingsParent;
        public Transform projectilesParent;
        public Transform routesParent;
        public int unitsAmount;
        public float spawnCooldown = 1f;
        public string mobId;
        public Transform[] spawnPoints;
        public string[] baseBuildingIds;

        [Header("UI")]
        public Canvas canvas;
        public BarsView barsView;
        public GameView gameView;
        public BuildView buildView;

        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsEntity _playerEntity;

        private RouteStorage _routeStorage;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            var staticData = new StaticDataProvider();
            staticData.Init();

#if UNITY_EDITOR
            EcsWorldObserver.Create(_world);
            EcsSystemsObserver.Create(_systems);
#endif

            InitPlayer();
            InitRoutes();
            InitUI(staticData);

            _systems

                // UNITS
                .Add(new UnitViewSystem(unitPrefabs, unitsParent))
                .Add(new UnitMovementSystem())
                .Add(new UnitViewUpdateSystem())
                .Add(new UnitAnimationSystem())
                .Add(new UnitHideSystem(_world))

                //TOWERS
                .Add(new TowerTargetsInRadiusUpdateSystem())
                .Add(new TowerSearchTargetSystem())
                .Add(new TowerAttackCooldownSystem())
                .Add(new TowerAttackSystem(_world))

                // //PROJECTILES
                .Add(new ProjectileViewSystem(projectilePrefab, projectilesParent))
                .Add(new ProjectileMovementSystem())
                .Add(new ProjectileViewUpdateSystem())

                .Add(new HealthSystem(_world))

                // GAME
                .Add(new CurrencySystem(_playerEntity))
                .Add(new WaveSystem(_world, staticData))
                .Add(new UnitSpawnSystem(_world, _routeStorage))
                .Add(new PlacementInitSystem(_world, staticData))
                .Add(new PlacementUpdateSystem(_world))
                .Add(new BuildSpawnSystem(_world))
                .Add(new BuildingViewSystem(_world, buildingsParent))

                // UI
                .Add(new InputProcessingUISystem(_world))
                .Add(new HealthBarUpdateSystem(barsView))
                .Add(new CurrencyUpdateSystem(gameView))
                ;

            _systems.Init();

            StartWave();
        }

        private void InitUI(StaticDataProvider staticData)
        {
            barsView.Init();
            barsView.InitCanvas(canvas);
            buildView.Init(_world);

            var baseBuildings = new List<BuildingPoolData>();
            foreach (var b in baseBuildingIds)
            {
                baseBuildings.Add(staticData.GetBuildingData(b));
            }
            buildView.InitSlots(baseBuildings.ToArray());
        }

        private void InitPlayer()
        {
            _playerEntity = _world.NewEntity();

            ref var currency = ref _playerEntity.Get<PlayerCurrencyComponent>();
        }

        private void InitRoutes()
        {
            _routeStorage = new RouteStorage();

            RouteAuthoring[] route = routesParent.GetComponentsInChildren<RouteAuthoring>();

            foreach (var r in route)
            {
                r.Initialize(_routeStorage);
            }
        }

        private void StartWave()
        {
            var waveEntity = _world.NewEntity();
            ref var wave = ref waveEntity.Get<WaveComponent>();
            wave.mobId = mobId;
            wave.spawnPos = spawnPoints[0].position;
            wave.spawnPeriod = spawnCooldown;
            wave.amount = unitsAmount;
        }

        private void Update()
        {
            _systems.Run();
        }

        private void OnDestroy()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}
