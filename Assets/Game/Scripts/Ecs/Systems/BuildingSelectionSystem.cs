using Leopotam.Ecs;

namespace TD
{
    public class BuildingSelectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<SelectBuildingRequest> _select;
        private readonly EcsFilter<DeselectBuildingRequest> _deselect;

        private EcsEntity _gameStateEntity;

        public BuildingSelectionSystem(EcsEntity gameStateEntity)
        {
            _gameStateEntity = gameStateEntity;
        }

        public void Run()
        {
            foreach (var i in  _select)
            {
                var select = _select.Get1(i);
                ref var gameState = ref _gameStateEntity.Get<GameStateComponent>();

                if (gameState.hasSelectedBuilding 
                    && gameState.selectedBuilding != null)
                {
                    if (gameState.selectedBuilding == select.entity)
                    {
                        _select.GetEntity(i).Destroy();
                        continue;
                    }
                    else
                    {
                        ToggleSelectedState(gameState.selectedBuilding, false);
                    }
                }

                gameState.selectedBuilding = select.entity;
                gameState.hasSelectedBuilding = true;
                ToggleSelectedState(select.entity, true);

                _select.GetEntity(i).Destroy();
            }

            foreach (var i in _deselect)
            {
                ref var gameState = ref _gameStateEntity.Get<GameStateComponent>();

                if (gameState.hasSelectedBuilding)
                {
                    if (gameState.selectedBuilding != null)
                    {
                        ToggleSelectedState(gameState.selectedBuilding, false);
                        gameState.selectedBuilding = default;
                        gameState.hasSelectedBuilding = false;
                    }
                }

                _deselect.GetEntity(i).Destroy();
            }
        }

        private void ToggleSelectedState(EcsEntity entity, bool toggle)
        {
            if (entity.Has<BuildingViewComponent>())
            {
                var view = entity.Get<BuildingViewComponent>();
                view.view.ToggleSelectedState(toggle);
            }
        }
    }
}