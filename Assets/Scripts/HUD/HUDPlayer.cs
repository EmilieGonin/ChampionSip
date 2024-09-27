using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _golds;
    [SerializeField] private List<Counter> _counters;

    private ulong _id;

    private void Awake()
    {
        PlayerNetwork.OnCurrencyUpdate += PlayerNetwork_OnCurrencyUpdate;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnCurrencyUpdate -= PlayerNetwork_OnCurrencyUpdate;
    }

    public void Init(ulong id)
    {
        _id = id;
        _name.text = GameManager.Instance.Players[id].Name;

        foreach (Counter counter in _counters) counter.SetPlayerId(id);
    }

    private void PlayerNetwork_OnCurrencyUpdate(ulong id,Currency currency, int amount)
    {
        if (id != _id || currency != Currency.Golds) return;
        _golds.text = amount.ToString();
    }
}