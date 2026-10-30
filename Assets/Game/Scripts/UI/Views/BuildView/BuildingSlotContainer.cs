using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TD
{
    public class BuildingSlotContainer : MonoBehaviour
    {
        public event Action<string> EventClick;

        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Color notAvailableColor;

        public BuildingPoolData data { get; private set; }

        public void Init(BuildingPoolData data)
        {
            this.data = data;
            titleText.text = data.title;
            iconImage.sprite = data.icon;
        }

        public void UpdateAvailable(int[] currencies)
        {
            bool isAvailable = CurrencyUtility.CanPurchase(currencies, data.costs);

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
            EventClick?.Invoke(data.id);
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
