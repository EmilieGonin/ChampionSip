using System;
using System.Collections.Generic;

public enum Currency
{
    Golds,
    SipsToDrink,
    Sips,
    Shots
}

public class ModEconomy : Module
{
    public static event Action<Currency, int> OnCurrencyUpdate;

    public Dictionary<Currency, int> Currencies { get; private set; }

    private void Awake()
    {
        Economy.OnAddGold += (int amount) => AddCurrency(Currency.Golds, amount);
        Counter.OnCounterAdd += AddCurrency;
        Counter.OnCounterRemove += RemoveCurrency;
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
    }

    public override void Init()
    {
        Currencies = _manager.Data.GetCurrenciesData();
    }

    private void OnDestroy()
    {
        Economy.OnAddGold -= (int amount) => AddCurrency(Currency.Golds, amount);
        Counter.OnCounterAdd -= AddCurrency;
        Counter.OnCounterRemove -= RemoveCurrency;
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
    }

    public void AddCurrency(Currency currency, int amount)
    {
        Currencies[currency] += amount;
        OnCurrencyUpdate?.Invoke(currency, Currencies[currency]);

        switch (currency)
        {
            case Currency.Sips:
                AddCurrency(Currency.Golds, 1);
                break;
            case Currency.Shots:
                AddCurrency(Currency.Golds, 5);
                break;
        }
    }

    public void RemoveCurrency(Currency currency, int amount)
    {
        Currencies[currency] -= amount;
        OnCurrencyUpdate?.Invoke(currency, Currencies[currency]);
    }

    private void PlayerNetwork_OnChallengeCompleted(bool win)
    {
        if (win) AddCurrency(Currency.Golds, 10);
    }
}