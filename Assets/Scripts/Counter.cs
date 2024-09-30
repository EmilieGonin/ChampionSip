using System;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static event Action<Currency, int> OnCounterAdd;
    public static event Action<Currency, int> OnCounterRemove;

    [SerializeField] private TMP_Text _counterNumber;
    [SerializeField] private Currency _currency;
    [SerializeField] private bool _isOwner;

    private bool _shieldIsUp;
    private bool _challengeShieldIsUp;

    private int _counter = 0;
    private ulong _playerId;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate += EffectSO_OnActivate;
        EffectSO.OnDeactivate += EffectSO_OnDeactivate;
        EffectSO.OnInflict += EffectSO_OnInflict;

        if (_isOwner) return;
        PlayerNetwork.OnCurrencyUpdate += PlayerNetwork_OnCurrencyUpdate;
    }

    private void Start()
    {
        if (!_isOwner) return;
        _counter = GameManager.Instance.Currencies[_currency];
        _counterNumber.text = _counter.ToString();
        _playerId = GameManager.Instance.PlayerId;
    }

    private void Update() => _counterNumber.text = _counter.ToString();

    public void SetPlayerId(ulong id)
    {
        _playerId = id;
        _counter = GameManager.Instance.Players[id].Currencies[_currency];
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate -= EffectSO_OnActivate;
        EffectSO.OnDeactivate -= EffectSO_OnDeactivate;
        EffectSO.OnInflict -= EffectSO_OnInflict;

        if (_isOwner) return;
        PlayerNetwork.OnCurrencyUpdate -= PlayerNetwork_OnCurrencyUpdate;
    }

    private void EffectSO_OnActivate(EffectSO effect, ulong id)
    {
        if (!_isOwner) return;
        if (effect is SipTransfer && _currency == Currency.SipsToDrink) RemoveCounter(5);
        else if (effect is ChallengeShield) _challengeShieldIsUp = true;
        else if (effect is Shield) _shieldIsUp = true;
    }

    private void EffectSO_OnDeactivate(EffectSO effect)
    {
        if (effect is Shield) _shieldIsUp = false;
    }

    private void EffectSO_OnInflict(EffectSO effect)
    {
        if (!_isOwner) return;
        if (effect is SipTransfer && _currency == Currency.SipsToDrink) AddCounter(5);
        else if (effect is Tsunami && _currency == Currency.SipsToDrink) AddCounter(10);
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        if (!_isOwner || _currency != Currency.SipsToDrink || victory || _shieldIsUp) return;

        if (_challengeShieldIsUp)
        {
            _challengeShieldIsUp = false;
            return;
        }

        AddCounter(5);
    }

    private void PlayerNetwork_OnCurrencyUpdate(ulong id, Currency currency, int amount)
    {
        if (id != _playerId || currency != _currency) return;
        _counter = amount;
    }

    public void AddCounter(int amount = 1)
    {
        if (_currency == Currency.SipsToDrink && _shieldIsUp) return;
        if (GameManager.Instance.HasEffect<DoubleSip>() && _currency == Currency.SipsToDrink) amount *= 2;
        _counter += amount;
        if (_isOwner) OnCounterAdd?.Invoke(_currency, amount);
    }

    public void RemoveCounter(int amount = 1)
    {
        if (_counter == 0) return;
        _counter -= amount;
        OnCounterRemove?.Invoke(_currency, amount);
    }
}