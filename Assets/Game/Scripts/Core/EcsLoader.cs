using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;

namespace TD
{
    public class EcsLoader : MonoBehaviour
    {
        public UnitView[] unitPrefabs;
        public ProjectileView projectilePrefab;
        public Transform unitsParent;
        public Transform projectilesParent;
        public Transform routesParent;
        public int unitsAmount;
        public float spawnCooldown = 1f;
        public Transform[] spawnPoints;
        public TriggerObserver[] towers;

        [Header("UI")]
        public Canvas canvas;
        public BarsView barsView;
        public GameView gameView;

        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsEntity _playerEntity;

        private RouteStorage _routeStorage;

        private float _spawnCooldownTimer;
        private int _spawnedUnitsCount;

        private void Start()
        {
            _world = new EcsWorld();

            InitPlayer();
            InitRoutes();
            InitUI();

            _systems = new EcsSystems(_world);
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

                // UI
                .Add(new HealthBarUpdateSystem(barsView))

                // GAME
                .Add(new CurrencySystem(_playerEntity))
                ;

            InitTowers();

            _systems.Init();
        }

        private void InitUI()
        {
            barsView.Init();
            barsView.InitCanvas(canvas);
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

        private void InitTowers()
        {
            foreach (var t in towers)
            {
                var entity = _world.NewEntity();

                entity.Get<TowerTargetsComponent>().targetsInRadius =
                    new HashSet<EcsEntity>();

                entity.Get<TowerAttackComponent>() = new TowerAttackComponent
                {
                    muzzleTrans = t.muzzleTrans,
                    attackCooldownDuration = 1f,
                    attackCooldownTimer = 0f
                };

                entity.Get<UnitPosition>() = new UnitPosition()
                {
                    value = t.transform.position
                };

                t.Init(_world, entity);
            }
        }

        private void SpawnUnit(int id)
        {
            if (_spawnedUnitsCount >= unitsAmount)
            {
                return;
            }

            var entity = _world.NewEntity();

            ref var movement = ref entity.Get<UnitMovement>();
            movement.routeId = id;
            movement.routeStorage = _routeStorage;
            movement.pointIndex = 0;
            movement.speed = 3f;

            ref var position = ref entity.Get<UnitPosition>();
            position.value = spawnPoints[id].position;
            _spawnedUnitsCount += 1;

            ref var state = ref entity.Get<UnitStateComponent>();
            state.unitId = Random.Range(0, unitPrefabs.Length);

            ref var hide = ref entity.Get<UnitHideComponent>();

            entity.Get<UnitHealthComponent>() = new UnitHealthComponent()
                {
                    baseHealthValue = 5,
                    currentHealthValue = 5
                };
        }

        private void Update()
        {
            _systems.Run();

            _spawnCooldownTimer += Time.deltaTime;

            if (_spawnCooldownTimer > spawnCooldown)
            {
                _spawnCooldownTimer -= spawnCooldown;
                SpawnUnit(Random.Range(0, unitPrefabs.Length));
            }
        }

        private void OnDestroy()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}
