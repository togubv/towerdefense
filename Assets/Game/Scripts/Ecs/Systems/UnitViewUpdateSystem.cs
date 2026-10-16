using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class UnitViewUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitPosition, UnitViewComponent,
            UnitMovement> _filter;

        private readonly float _routeOffset = 0.05f;
        private readonly float _clamp = 5f;

        public void Run()
        {
            foreach (var i in _filter)
            {
                var entity = _filter.GetEntity(i);                

                ref var position = ref _filter.Get1(i);
                ref var view = ref _filter.Get2(i);
                ref var movement = ref _filter.Get3(i);

                if (entity.Has<IsDeadTag>())
                {
                    if (view.view != null)
                    {
                        Object.Destroy(view.view.gameObject);
                    }

                    continue;
                }

                if (position.rotation != Vector3.zero)
                {
                    view.trans.rotation = Quaternion.LookRotation(position.rotation);
                }

                Vector3 targetOffset = view.trans.right * _routeOffset;
                view.currentOffset = Vector3.Lerp(view.currentOffset,
                    targetOffset,
                    _clamp * Time.deltaTime);

                view.trans.position = position.value + view.currentOffset;
            }
        }
    }
}
