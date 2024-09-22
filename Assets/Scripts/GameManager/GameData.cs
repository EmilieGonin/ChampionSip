using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int Golds;
    public int SipsToDrink;
    public int Sips;
    public int Shots;

    public Dictionary<Currency, int> GetCurrenciesData()
    {
        Dictionary<Currency, int> currencies = new();

        foreach (Currency currency in Enum.GetValues(typeof(Currency)))
        {
            switch (currency)
            {
                case Currency.Golds:
                    currencies[currency] = Golds;
                    break;
                case Currency.SipsToDrink:
                    currencies[currency] = SipsToDrink;
                    break;
                case Currency.Sips:
                    currencies[currency] = Sips;
                    break;
                case Currency.Shots:
                    currencies[currency] = Shots;
                    break;
            }
        }

        return currencies;
    }
}