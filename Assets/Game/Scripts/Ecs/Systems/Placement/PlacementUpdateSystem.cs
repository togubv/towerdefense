using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TD
{
    public class PlacementUpdateSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilter<PlacementPreviewComponent> _previews;

        private EcsWorld _world;
        private Camera _mainCamera;
        private LayerMask _groundLayerMask;

        public PlacementUpdateSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Init()
        {
            _mainCamera = Camera.main;
            _groundLayerMask = 1 << 8;
        }

        public void Run()
        {
            if (_previews.IsEmpty())
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _groundLayerMask))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.x = Mathf.Round(targetPosition.x);
                targetPosition.z = Mathf.Round(targetPosition.z);

                foreach (var i in _previews)
                {
                    ref var preview = ref _previews.Get1(i);

                    if (preview.previewTrans != null)
                    {
                        preview.previewTrans.position = targetPosition;
                    }

                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        // Создаем запрос на реальную постройку здания
                        var requestEntity = _world.NewEntity();
                        ref var buildRequest = ref requestEntity.Get<BuildStructureRequest>();
                        buildRequest.worlsPos = targetPosition;
                        buildRequest.data = preview.data;

                        // Удаляем призрак
                        Object.Destroy(preview.previewTrans.gameObject);
                        _previews.GetEntity(i).Destroy();
                    }

                    // Отмена на ПКМ
                    if (Mouse.current.rightButton.wasPressedThisFrame)
                    {
                        Object.Destroy(preview.previewTrans.gameObject);
                        _previews.GetEntity(i).Destroy();
                    }
                }
            }
        }
    }
}