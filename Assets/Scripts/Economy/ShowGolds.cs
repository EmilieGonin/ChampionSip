using TMPro;
using UnityEngine;

public class ShowGolds : MonoBehaviour
{
    [SerializeField] private TMP_Text _gold;

    private int _goldInt;

    private void Awake()
    {
        ModEconomy.OnCurrencyUpdate += ModEconomy_OnCurrencyUpdate;
    }

    private void OnDestroy()
    {
        ModEconomy.OnCurrencyUpdate -= ModEconomy_OnCurrencyUpdate;
    }

    private void ModEconomy_OnCurrencyUpdate(Currency currency, int amount)
    {
        if (currency != Currency.Golds) return;
        _goldInt = amount;
        _gold.text = _goldInt.ToString();
    }
}