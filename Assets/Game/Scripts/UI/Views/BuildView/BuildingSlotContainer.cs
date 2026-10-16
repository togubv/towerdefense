using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TD
{
    public class BuildingSlotContainer : MonoBehaviour
    {
        public event Action<string> EventClick;

        public Button button;
        public Image iconImage;
        public TMP_Text titleText;

        public string buildingId { get; private set; }

        public void Init(string buildingId, string title, Sprite icon)
        {
            this.buildingId = buildingId;
            titleText.text = title;
            iconImage.sprite = icon;
        }
        
        private void ClickHandler()
        {
            EventClick?.Invoke(buildingId);
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
