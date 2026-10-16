namespace TD
{
    public struct CurrencyChangedEvent
    {
        public CurrencyType currency;
        public int value;

        public CurrencyChangedEvent(CurrencyType currency, int value)
        {
            this.currency = currency;
            this.value = value;
        }
    }
}