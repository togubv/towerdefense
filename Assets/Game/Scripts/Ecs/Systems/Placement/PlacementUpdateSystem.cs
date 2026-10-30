using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TD
{
    /// <summary>
    /// Обновляет позицию превью постройки, при клике создает запрос на постройку и тратит стоимость.
    /// </summary>
    public class PlacementUpdateSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilter<PlacementPreviewComponent> _previews;

        private readonly EcsWorld _world;
        private readonly EcsEntity _playerEntity;
        private Camera _mainCamera;
        private readonly int _layerMask = (1 << 8) | (1 << 9);

        public PlacementUpdateSystem(EcsWorld world, EcsEntity playerEntity)
        {
            _world = world;
            _playerEntity = playerEntity;
        }

        public void Init()
        {
            _mainCamera = Camera.main;
        }

        public void Run()
        {
            if (_previews.IsEmpty())
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _layerMask))
            {
                if (hit.collider.gameObject.layer == 8)
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
                            preview.view.SetUnplaceableState(false);
                        }

                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            // Создаем запрос на реальную постройку здания
                            var requestEntity = _world.NewEntity();
                            ref var buildRequest = ref requestEntity.Get<BuildStructureRequest>();
                            buildRequest.worlsPos = targetPosition;
                            buildRequest.data = preview.data;

                            // Тратим валюту игрока от ценника постройки 
                            if (preview.data.costs != null)
                            {
                                var playerCurrency = _playerEntity.Get<PlayerCurrencyComponent>();
                                if (CurrencyUtility.CanPurchase(playerCurrency, preview.data.costs))
                                {
                                    var changeCurrencyEntity = _world.NewEntity();
                                    ref var changeCurrency = ref changeCurrencyEntity.Get<DecreaseCurrencyCommand>();
                                    changeCurrency.amount = preview.data.costs;
                                }
                            }

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
                else
                {
                    Vector3 targetPosition = hit.point;

                    foreach (var i in _previews)
                    {
                        ref var preview = ref _previews.Get1(i);

                        if (preview.previewTrans != null)
                        {
                            preview.previewTrans.position = targetPosition;
                            preview.view.SetUnplaceableState(true);
                        }

                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            Debug.Log("CANT PLACE!");
                        }
                    }
                }
            }
        }
    }
}