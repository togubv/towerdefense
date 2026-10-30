using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TD
{
    public class BuildingUpgradeSlotContainer : MonoBehaviour
    {
        public event Action<BuildingUpgradeData> EventClick;

        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Color notAvailableColor;

        public BuildingUpgradeData upgradeData { get; private set; }

        public void Init(BuildingUpgradeData data)
        {
            upgradeData = data;
            titleText.text = upgradeData.data.title;
            iconImage.sprite = upgradeData.data.icon;
        }

        public void UpdateAvailable(int[] currencies)
        {
            if (upgradeData == null)
            {
                return;
            }

            bool isAvailable = CurrencyUtility.CanPurchase(currencies, upgradeData.costs);

            if (isAvailable)
            {
                iconImage.color = Color.white;
            }
            else
            {
                iconImage.color = notAvailableColor;
            }

            button.interactable = isAvailable;
        }

        private void ClickHandler()
        {
            EventClick?.Invoke(upgradeData);
        }

        private void OnEnable()
        {
            button.onClick.AddListener(ClickHandler);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(ClickHandler);
        }
    }
}
