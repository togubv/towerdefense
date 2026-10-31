using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class CameraMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CameraMovementComponent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var movement = ref _filter.Get1(i);

                // MOVEMENT

                Vector2 input = movement.input;

                if (input.sqrMagnitude > 0f)
                {
                    float angle = movement.orbitalFollow.HorizontalAxis.Value;
                    Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 forward = rotation * Vector3.forward;
                    Vector3 right = rotation * Vector3.right;
                    Vector3 direction = forward * input.y + right * input.x;

                    direction = Vector3.ClampMagnitude(direction, 1f);

                    movement.trans.position += movement.speed * Time.deltaTime * direction;
                }

                // ROTATE 

                if (movement.orbitalFollow != null && movement.rotateInput != 0f)
                {
                    var axis = movement.orbitalFollow.HorizontalAxis;

                    axis.Value += movement.rotateInput *
                                  movement.rotateSpeed *
                                  Time.deltaTime;

                    axis.Value = Mathf.Repeat(axis.Value + 180f, 360f) - 180f;

                    movement.orbitalFollow.HorizontalAxis = axis;
                }
            }
        }
    }
}