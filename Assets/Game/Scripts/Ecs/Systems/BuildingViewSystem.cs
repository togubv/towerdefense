using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class BuildingViewSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingViewWaiter> _filter;

        private readonly Transform _parent;
        private readonly EcsWorld _world;

        public BuildingViewSystem(EcsWorld world, Transform parent)
        {
            _world = world;
            _parent = parent;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var viewWaiter = ref _filter.Get1(i);
                ref var entity = ref _filter.GetEntity(i);

                var view = Object.Instantiate(viewWaiter.poolData.prefab, viewWaiter.spawnPos,
                    Quaternion.identity, _parent);

                ref var viewComponent = ref entity.Get<BuildingViewComponent>();
                viewComponent.entity = entity;
                viewComponent.trans = view.transform;
                viewComponent.view = view;
                entity.Get<TowerAttackComponent>().muzzleTrans = view.triggerObserver.muzzleTrans;

                view.triggerObserver.Init(_world, entity);

                entity.Del<BuildingViewWaiter>();
            }
        }
    }
}