using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TD
{
    public class InputReader : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private EcsEntity _cameraEntity;

        public void Init(EcsEntity cameraEntity)
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();

            _playerInput.Player.CameraMove.performed += CameraMoveHandler;
            _playerInput.Player.CameraMove.canceled += CameraMoveCanceledHandler;

            _cameraEntity = cameraEntity;
        }

        private void OnDestroy()
        {
            if (_playerInput != null)
            {
                _playerInput.Player.CameraMove.performed -= CameraMoveHandler;
                _playerInput.Player.CameraMove.canceled -= CameraMoveCanceledHandler;

                _playerInput.Disable();
            }
        }

        private void CameraMoveHandler(InputAction.CallbackContext ctx)
        {
            ref var movement = ref _cameraEntity.Get<CameraMovementComponent>();
            movement.input = ctx.ReadValue<Vector2>();
        }

        private void CameraMoveCanceledHandler(InputAction.CallbackContext ctx)
        {
            ref var movement = ref _cameraEntity.Get<CameraMovementComponent>();
            movement.input = Vector2.zero;
        }
    }
}