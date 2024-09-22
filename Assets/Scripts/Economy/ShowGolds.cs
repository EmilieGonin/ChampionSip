using TMPro;
using UnityEngine;

public class ShowGolds : MonoBehaviour
{
    [SerializeField] private TMP_Text _gold;

    private void Awake()
    {
        ModEconomy.OnCurrencyUpdate += ModEconomy_OnCurrencyUpdate;
        _gold.text = GameManager.Instance.Currencies[Currency.Golds].ToString();
    }

    private void OnDestroy()
    {
        ModEconomy.OnCurrencyUpdate -= ModEconomy_OnCurrencyUpdate;
    }

    private void ModEconomy_OnCurrencyUpdate(Currency currency, int amount)
    {
        if (currency != Currency.Golds) return;
        _gold.text = amount.ToString();
    }
}