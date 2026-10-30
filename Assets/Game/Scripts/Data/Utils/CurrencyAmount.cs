using System;

namespace TD
{
    [Serializable]
    public struct CurrencyAmount
    {
        public CurrencyType currency;
        public int value;
    }
}
