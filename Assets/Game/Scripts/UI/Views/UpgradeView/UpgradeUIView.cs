using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace TD
{
    public class UpgradeUIView : MonoBehaviour
    {
        [SerializeField] private RectTransform _upgradeContainer;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private BuildingUpgradeSlotContainer[] _upgradeSlots;

        private EcsWorld _world;
        private RectTransform _canvasRectT;
        private Camera _cam;

        private EcsEntity _selectedBuildingEntity;

        public void Init(EcsWorld world)
        {
            _world = world;
            _cam = Camera.main;
        }

        public void InitCanvas(Canvas canvas)
        {
            _cam = Camera.main;
            _canvasRectT = canvas.GetComponent<RectTransform>();
        }

        public void ShowUpgrades(Vector3 worldPos,
            BuildingPoolData building, EcsEntity upgradeBuildingEntity)
        {
            titleText.text = building.title;

            var screenPos = GetScreenPosition(worldPos);
            _upgradeContainer.anchoredPosition = screenPos;
            _upgradeContainer.gameObject.SetActive(true);

            _selectedBuildingEntity = upgradeBuildingEntity;

            for (int i = 0; i < _upgradeSlots.Length; i++)
            {
                if (i < building.upgrades.Length)
                {
                    _upgradeSlots[i].Init(building.upgrades[i]);
                    _upgradeSlots[i].gameObject.SetActive(true);
                }
                else
                {
                    _upgradeSlots[i].gameObject.SetActive(false);
                }
            }
        }

        public void HideUpgrades()
        {
            _selectedBuildingEntity = default;
            _upgradeContainer.gameObject.SetActive(false);
        }

        public void UpdateBuildingsAvailables(int[] amounts)
        {
            foreach (var b in _upgradeSlots)
            {
                b.UpdateAvailable(amounts);
            }
        }

        private void UpgradeBuildingClickHandler(BuildingUpgradeData upgradeData)
        {
            var requestEntity = _world.NewEntity();
            ref var upgradeRequest = ref requestEntity.Get<BuildingUpgradeRequest>();
            upgradeRequest.buildingEntity = _selectedBuildingEntity;
            upgradeRequest.upgradeData = upgradeData;
        }

        private void OnEnable()
        {
            AddHandlers();
        }

        private void OnDisable()
        {
            RemoveHandlers();
        }

        private void AddHandlers()
        {
            foreach (var b in _upgradeSlots)
            {
                b.EventClick += UpgradeBuildingClickHandler;
            }
        }

        private void RemoveHandlers()
        {
            foreach (var b in _upgradeSlots)
            {
                b.EventClick -= UpgradeBuildingClickHandler;
            }
        }

        private Vector2 GetScreenPosition(Vector3 worldPos)
        {
            Vector2 screenPos = _cam.WorldToScreenPoint(
                worldPos
                );

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRectT,
                screenPos,
                _cam,
                out Vector2 localPos
            );

            return localPos;
        }
    }
}

