using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TD
{
    public class ObjectClickInput : MonoBehaviour
    {
        [SerializeField] private LayerMask _buildingLayer;
        [SerializeField] private LayerMask _groundLayer;

        private EcsWorld _world;
        private Camera _camera;

        public void Init(EcsWorld world)
        {
            _world = world;
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current == null)
            {
                return;
            }

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

                var eventEntity = _world.NewEntity();

                if (Physics.Raycast(ray, out RaycastHit buildingHit, 100f, _buildingLayer))
                {
                    if (buildingHit.collider.TryGetComponent(out BuildingView buildingView))
                    {
                        ref var clickEvent = ref eventEntity.Get<ObjectClickedEvent>();
                        clickEvent.view = buildingView;
                        clickEvent.clickWorldPos = buildingView.transform.position;
                        Debug.Log(buildingView.name);
                        return;
                    }
                }

                ref var nullEvent = ref eventEntity.Get<ObjectClickedEvent>();
                nullEvent.view = null;

                if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, _groundLayer))
                {
                    eventEntity.Get<GroundClickedEvent>();
                    return;
                }
            }
        }
    }
}