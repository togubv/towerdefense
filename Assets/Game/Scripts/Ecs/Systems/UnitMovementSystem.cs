using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class UnitMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitMovement, UnitPosition, UnitStateComponent>
            .Exclude<IsDeadTag> _filter;

        private readonly EcsWorld _world;

        public UnitMovementSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var movement = ref _filter.Get1(i);
                ref var position = ref _filter.Get2(i);
                ref var state = ref _filter.Get3(i);

                var path = movement.path;

                if (state.current == UnitStateType.Hidden ||
                    state.current == UnitStateType.Hide)
                {
                    continue;
                }

                if (path == null || path.Length < 1)
                {
                    continue;
                }

                if (movement.pathIndex > path.Length - 1)
                {
                    if (path[0] == path[^1])
                    {
                        movement.pathIndex = 0;
                    }
                    else
                    {
                        var decreaseLiveEntity = _world.NewEntity();
                        ref var decreaseCurrency = ref decreaseLiveEntity.Get<DecreaseCurrencyCommand>();
                        decreaseCurrency.amount = new CurrencyAmount[]
                            {
                                new() { currency = CurrencyType.Lives, value = 1 }
                            };

                        ref var deadTag = ref _filter.GetEntity(i).Get<IsDeadTag>();
                        deadTag.withAnimation = false;
                        continue;
                    }
                }

                var targetVector2Int = path[movement.pathIndex];

                if (movement.isReturn)
                {
                    targetVector2Int = path[path.Length - 1 - movement.pathIndex];
                }
                else
                {
                    targetVector2Int = path[movement.pathIndex];
                }

                var target = new Vector3(targetVector2Int.x, position.value.y, targetVector2Int.y);
                Vector3 direction = target - position.value;

                float distance = direction.magnitude;
                float step = movement.speed * Time.deltaTime;

                if (distance <= step)
                {
                    position.value = target;
                    movement.pathIndex += 1;
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
