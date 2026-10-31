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

                Vector2 input = movement.input;
                
                if (input.sqrMagnitude == 0f)
                {
                    continue;
                }

                Transform cameraTrans = movement.trans;
                Vector3 forward = cameraTrans.forward;
                forward.y = 0f;
                forward.Normalize();

                Vector3 right = cameraTrans.right;
                right.y = 0f;
                forward.Normalize();

                Vector3 direction = forward * input.y + right * input.x;

                direction = Vector3.ClampMagnitude(direction, 1f);

                cameraTrans.position += direction * movement.speed * Time.deltaTime;
            }
        }
    }
}