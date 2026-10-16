using TMPro;
using UnityEngine;

namespace TD
{
    public class CurrencyContainer : MonoBehaviour
    {
        [SerializeField] private CurrencyType currency;
        [SerializeField] private TMP_Text valueText;

        public void SetValue(int value)
        {
            valueText.text = value.ToString();
        }

        public CurrencyType GetCurrencyType()
        {
            return currency;
        }
    }
}
