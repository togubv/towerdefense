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
        public VoxelsGenerator voxelsGenerator;
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
        public Vector2Int destinationCell;
        public CurrencyAmount[] startCurrencies;

        [Header("Managers")]
        public ObjectClickInput objectClickInput;

        [Header("UI")]
        public Canvas canvas;
        public BarsUIView barsView;
        public GameUIView gameView;
        public BuildUIView buildView;
        public UpgradeUIView upgradeView;

        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsEntity _playerEntity;

        private GridManager _gridManager;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            objectClickInput.Init(_world);

            var staticData = new StaticDataProvider();
            staticData.Init();

            _gridManager = new GridManager(
                _world,
                voxelsGenerator.matrixSize.x,
                voxelsGenerator.matrixSize.y,
                destinationCell);

#if UNITY_EDITOR
            EcsWorldObserver.Create(_world);
            EcsSystemsObserver.Create(_systems);
#endif

            InitPlayer();
            InitUI(staticData);

            _systems

                .Add(new UnitSpawnSystem(_world, _gridManager))

                .Add(new PathUpdateSystem(destinationCell))

                // UNITS
                .Add(new UnitViewSystem(unitPrefabs, unitsParent))
                .Add(new UnitMovementSystem(_world))
                .Add(new UnitViewUpdateSystem(barsView))
                .Add(new UnitAnimationSystem())

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
                .Add(new CurrencySystem(_world, _playerEntity))
                .Add(new WaveSystem(_world, staticData))

                .Add(new BuildRequestSystem(_world, _playerEntity, staticData))
                .Add(new PlacementInitSystem(_world, staticData))
                .Add(new PlacementUpdateSystem(_world, _playerEntity))
                .Add(new BuildSpawnSystem(_world, _gridManager))
                .Add(new BuildingViewSystem(_world, buildingsParent))
                .Add(new ObjectClickedSystem(upgradeView, staticData, _playerEntity))
                .Add(new BuildingUpgradeSystem(_world, _playerEntity))

                // UI
                .Add(new InputProcessingUISystem(_world))
                .Add(new HealthBarUpdateSystem(barsView))
                .Add(new CurrencyUpdateSystem(_playerEntity, gameView,
                buildView, upgradeView))
                ;

            _systems.Init();

            AddDebugCurrencies();
            AddLevelLives(30);
            StartWave();
        }

        private void InitUI(StaticDataProvider staticData)
        {
            barsView.Init();
            barsView.InitCanvas(canvas);
            buildView.Init(_world);
            upgradeView.Init(_world);
            upgradeView.InitCanvas(canvas);

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

            _playerEntity.Get<PlayerCurrencyComponent>() =
                new PlayerCurrencyComponent
                {
                    amount = new int[(int)CurrencyType.Count]
                };
        }

        private void AddLevelLives(int value)
        {
            var addingEntity = _world.NewEntity();
            ref var adding = ref addingEntity.Get<IncreaseCurrencyCommand>();
            adding.amount = new CurrencyAmount[]
                {
                    new() { currency = CurrencyType.Lives, value = value }
                };
        }

        private void AddDebugCurrencies()
        {
            if (startCurrencies == null || startCurrencies.Length == 0)
            {
                return;
            }

            var addingEntity = _world.NewEntity();
            ref var adding = ref addingEntity.Get<IncreaseCurrencyCommand>();
            adding.amount = startCurrencies;
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
