using System;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static event Action<int> OnNewSip;

    [SerializeField] private TMP_Text _counterNumber;
    [SerializeField] private bool _isStat;

    private bool _shieldIsUp;
    private bool _challengeShieldIsUp;

    private int _counter = 0;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate += EffectSO_OnActivate;
        EffectSO.OnDeactivate += EffectSO_OnDeactivate;
        EffectSO.OnInflict += EffectSO_OnInflict;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate -= EffectSO_OnActivate;
        EffectSO.OnDeactivate -= EffectSO_OnDeactivate;
        EffectSO.OnInflict -= EffectSO_OnInflict;
    }

    private void EffectSO_OnActivate(EffectSO effect)
    {
        if (effect is SipTransfer && !_isStat) RemoveCounter(5);
        else if (effect is ChallengeShield) _challengeShieldIsUp = true;
        else if (effect is Shield) _shieldIsUp = true;
    }

    private void EffectSO_OnDeactivate(EffectSO effect)
    {
        if (effect is Shield) _shieldIsUp = false;
    }

    private void EffectSO_OnInflict(EffectSO effect)
    {
        if (!_isStat && effect is SipTransfer) AddCounter(5);
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        if (_isStat || victory || _shieldIsUp) return;

        if (_challengeShieldIsUp)
        {
            _challengeShieldIsUp = false;
            return;
        }

        AddCounter(5);
    }

    public void AddCounter(int amount = 1)
    {
        if (!_isStat && _shieldIsUp) return;
        _counter += amount;
        _counterNumber.text = _counter.ToString();
        if (!_isStat) OnNewSip?.Invoke(amount);
    }

    public void RemoveCounter(int amount = 1)
    {
        _counter -= amount;
        if (_counter < 0) _counter = 0;
        _counterNumber.text = _counter.ToString();
    }
}