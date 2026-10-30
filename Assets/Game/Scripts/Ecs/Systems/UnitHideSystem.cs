using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public class UnitHideSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitStateComponent,
            UnitViewComponent, UnitHideComponent, 
            UnitMovement> _filter;

        private Vector2 _hidingDurationRange = new Vector2(5f, 10f);

        private readonly EcsWorld _world;

        public UnitHideSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var state = ref _filter.Get1(i);
                ref var view = ref _filter.Get2(i);
                ref var hide = ref _filter.Get3(i);
                ref var movement = ref _filter.Get4(i);

                if (state.current == UnitStateType.Hide)
                {
                    hide.hideDuration = Random.Range(
                        _hidingDurationRange.x,
                        _hidingDurationRange.y);

                    state.current = UnitStateType.Hidden;
                    view.view.gameObject.SetActive(false);
                }
                else if (state.current == UnitStateType.Hidden)
                {
                    hide.hideTimer += Time.deltaTime;

                    if (hide.hideTimer > hide.hideDuration)
                    {
                        //movement.pointIndex = 0;
                        view.view.gameObject.SetActive(true);
                        state.current = UnitStateType.Idle;
                        hide.hideTimer = 0;

                        //if (movement.isReturn == false)
                        //{
                        //    AddCurrency(state.insideBuildingType);
                        //}

                        movement.isReturn = !movement.isReturn;
                        // state.insideBuildingType = BuildingType.None;
                    }
                }
            }
        }

        //private void AddCurrency(BuildingType insideBuilding)
        //{
        //    var entity = _world.NewEntity();
        //    ref var change = ref entity.Get<ChangeCurrencyCommand>();

        //    switch (insideBuilding)
        //    {
        //        case BuildingType.Mill:
        //            change.type = CurrencyType.Food;
        //            break;

        //        case BuildingType.Sawmill:
        //            change.type = CurrencyType.Wood;
        //            break;
        //    }

        //    change.value = 1;
        //}
    }
}
