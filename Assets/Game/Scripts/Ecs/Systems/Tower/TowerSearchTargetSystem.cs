using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class TowerSearchTargetSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitPosition, TowerTargetsComponent>
            .Exclude<TowerTargetComponent> _towersWithoutTarget;

        public void Run()
        {
            foreach (var i in _towersWithoutTarget)
            {
                var towerPos = _towersWithoutTarget.Get1(i);
                var targets = _towersWithoutTarget.Get2(i).targetsInRadius;

                float minDistance = float.MaxValue;
                EcsEntity closestTarget = default;

                foreach (var t in targets)
                {
                    if (t.IsAlive() == false || t.Has<UnitPosition>() == false)
                    {
                        continue;
                    }

                    if (t.Has<IsDeadTag>())
                    {
                        continue;
                    }

                    ref var targetPos = ref t.Get<UnitPosition>();
                    var distance = (targetPos.value - towerPos.value).sqrMagnitude;

                    if (distance >= minDistance)
                    {
                        continue;
                    }

                    minDistance = distance;
                    closestTarget = t;
                }

                if (closestTarget.IsAlive() == false)
                {
                    continue;
                }

                var tower = _towersWithoutTarget.GetEntity(i);
                ref var towerTarget = ref tower.Get<TowerTargetComponent>();
                towerTarget.target = closestTarget;
            }
        }
    }
}