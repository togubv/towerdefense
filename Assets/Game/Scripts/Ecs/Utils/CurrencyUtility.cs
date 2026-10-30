namespace TD
{
    public static class CurrencyUtility
    {
        public static int GetCurrencyValue(PlayerCurrencyComponent currencyComponent,
            CurrencyType currency)
        {
            return currencyComponent.amount[(int)currency];
        }

        public static bool CanPurchase(PlayerCurrencyComponent currencyComponent,
            CurrencyAmount[] costs)
        {
            return CanPurchase(currencyComponent.amount, costs);
        }

        public static bool CanPurchase(int[] amounts, CurrencyAmount[] costs)
        {
            if (costs == null)
            {
                return true;
            }

            foreach (var c in costs)
            {
                int index = (int)c.currency;

                if (c.value < 0 || index  < 0 || 
                    index >= amounts.Length)
                {
                    return false;
                }

                if (amounts[index] < c.value)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool TrySpend(ref PlayerCurrencyComponent currencyComponent,
            CurrencyAmount[] costs
            )
        {
            if (CanPurchase(currencyComponent, costs) == false)
            {
                return false;
            }

            foreach (var c in costs)
            {
                currencyComponent.amount[(int)c.currency] -= c.value;
            }

            return true;
        }

        public static void AddCurrency(ref PlayerCurrencyComponent currencyComponent,
            CurrencyAmount[] costs)
        {
            foreach (var c in costs)
            {
                currencyComponent.amount[(int)c.currency] += c.value;
            }
        }
    }
}