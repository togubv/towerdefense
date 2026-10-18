using TMPro;
using UnityEngine;

namespace TD
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private CurrencyContainer[] _currencyContainers;

        public void SetCurrencyValue(CurrencyType currency, int value)
        {
            if (currency == CurrencyType.Lives)
            {
                livesText.text = $"{value}\nLives";
            }
            else
            {
                var container = GetCurrencyContainer(currency);
                container.SetValue(value);
            }
        }

        private CurrencyContainer GetCurrencyContainer(CurrencyType currency)
        {
            foreach (var c in _currencyContainers)
            {
                if (c.GetCurrencyType() == currency)
                {
                    return c;
                }
            }

            return null;
        }
    }
}
