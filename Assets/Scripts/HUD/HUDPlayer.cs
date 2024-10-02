using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDPlayer : MonoBehaviour
{
    public static event Action<ulong> OnPlayerSelect;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _golds;
    [SerializeField] private List<Counter> _counters;
    [SerializeField] private HUDSipEffects _sipEffects;

    private ulong _id;

    private void Awake() => PlayerNetwork.OnCurrencyUpdate += PlayerNetwork_OnCurrencyUpdate;
    private void OnDestroy() => PlayerNetwork.OnCurrencyUpdate -= PlayerNetwork_OnCurrencyUpdate;

    public void Init(ulong id)
    {
        _id = id;
        _name.text = GameManager.Instance.Players[id].Name;
        _golds.text = GameManager.Instance.Players[id].Currencies[Currency.Golds].ToString();

        _sipEffects.SetPlayerId(id);
        foreach (Counter counter in _counters) counter.SetPlayerId(id);
    }

    private void PlayerNetwork_OnCurrencyUpdate(ulong id,Currency currency, int amount)
    {
        if (id != _id || currency != Currency.Golds) return;
        _golds.text = amount.ToString();
    }

    public void Click() => OnPlayerSelect?.Invoke(_id);
}