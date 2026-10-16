using System;
using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class UnitMovementSystem : IEcsRunSystem
    {
        private EcsFilter<UnitMovement, UnitPosition, UnitStateComponent>.Exclude<IsDeadTag> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var movement = ref _filter.Get1(i);
                ref var position = ref _filter.Get2(i);
                ref var state = ref _filter.Get3(i);

                var route = movement.routeStorage.Get(movement.routeId);

                if (state.current == UnitStateType.Hidden ||
                    state.current == UnitStateType.Hide)
                {
                    continue;
                }

                if (movement.pointIndex > route.points.Length - 1)
                {
                    if (route.points[0] == route.points[^1])
                    {
                        movement.pointIndex = 0;
                    }
                    else
                    {
                        state.current = UnitStateType.Hide;
                        continue;
                    }
                }

                Vector3 target = route.points[movement.pointIndex];

                if (movement.isReturn)
                {
                    target = route.points[route.points.Length - 1 - movement.pointIndex];
                }
                else
                {
                    target = route.points[movement.pointIndex];
                }

                Vector3 direction = target - position.value;

                float distance = direction.magnitude;
                float step = movement.speed * Time.deltaTime;

                if (distance <= step)
                {
                    position.value = target;
                    movement.pointIndex += 1;
                }
                else
                {
                    position.value += direction / distance * step;
                    direction.y = 0f;

                    if (position.rotation != direction)
                    {
                        position.rotation = direction;
                    }

                    state.current = UnitStateType.Move;
                }
            }
        }
    }
}
