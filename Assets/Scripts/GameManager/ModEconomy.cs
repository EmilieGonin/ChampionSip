using System;
using System.Collections.Generic;
using UnityEditor;

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

    public int Golds => Currencies[Currency.Golds];
    public int SipsToDrink => Currencies[Currency.SipsToDrink];

    public Dictionary<Currency, int> Currencies { get; private set; }

    private void Awake()
    {
        Economy.OnAddGold += (int amount) => AddCurrency(Currency.Golds, amount);
        Counter.OnCounterUpdate += AddCurrency;
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
    }

    private void Start()
    {
        Currencies = new();

        foreach (Currency value in Enum.GetValues(typeof(Currency)))
        {
            Currencies[value] = 0;
        }
    }

    private void OnDestroy()
    {
        Economy.OnAddGold -= (int amount) => AddCurrency(Currency.Golds, amount);
        Counter.OnCounterUpdate -= AddCurrency;
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
    }

    public void AddCurrency(Currency currency, int amount)
    {
        Currencies[currency] += amount;
        OnCurrencyUpdate?.Invoke(currency, Currencies[currency]);

        if (currency == Currency.SipsToDrink) AddCurrency(Currency.Sips, amount);
    }

    public void RemoveCurrency(Currency currency, int amount)
    {
        Currencies[currency] -= amount;
        OnCurrencyUpdate?.Invoke(currency, Currencies[currency]);

        if (currency == Currency.SipsToDrink) RemoveCurrency(Currency.Sips, amount);
    }

    private void PlayerNetwork_OnChallengeCompleted(bool win)
    {
        if (win) AddCurrency(Currency.Golds, 10);
    }
}