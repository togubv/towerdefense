using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class TriggerObserver : MonoBehaviour
    {
        public Transform muzzleTrans;

        private EcsWorld _world;
        private EcsEntity _entity;

        public void Init(EcsWorld world, EcsEntity entity)
        {
            _world = world;
            _entity = entity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_world == null)
            {
                return;
            }

            if (!_entity.IsAlive())
            {
                return;
            }

            if (other.TryGetComponent(out UnitView unitView) == false)
                return;

            if (!unitView.entity.IsAlive())
            {
                return;
            }

            var eventEntity = _world.NewEntity();
            ref var triggerEvent = ref eventEntity.Get<OnTriggerEnterEvent>();
            triggerEvent.ownerEntity = _entity;
            triggerEvent.enteredEntity = unitView.entity;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_world == null)
            {
                return;
            }

            if (!_entity.IsAlive())
            {
                return;
            }

            if (other.TryGetComponent(out UnitView unitView) == false)
            {
                return;
            }

            if (!unitView.entity.IsAlive())
            {
                return;
            }

            var eventEntity = _world.NewEntity();

            ref var triggerEvent = ref eventEntity.Get<OnTriggerExitEvent>();
            triggerEvent.ownerEntity = _entity;
            triggerEvent.exitedEntity = unitView.entity;
        }
    }
}