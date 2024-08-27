using System;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static event Action<int> OnNewSip;

    [SerializeField] private TMP_Text _counterNumber;
    [SerializeField] private bool _isStat;

    private int _counter = 0;
    private bool _isProtected;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
        PlayerNetwork.OnSipTransfer += PlayerNetwork_OnSipTransfer;
        EffectSO.OnActivate += EffectSO_OnActivate;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
        PlayerNetwork.OnSipTransfer -= PlayerNetwork_OnSipTransfer;
        EffectSO.OnActivate -= EffectSO_OnActivate;
    }

    private void PlayerNetwork_OnSipTransfer()
    {
        if (!_isStat) AddCounter(5);
    }

    private void EffectSO_OnActivate(EffectSO effect)
    {
        if (effect is ChallengeShield) _isProtected = true;
        if (effect is SipTransfer && !_isStat) RemoveCounter(5);
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        if (_isStat || victory) return;

        if (_isProtected)
        {
            _isProtected = false;
            return;
        }

        _counter += 5;
        _counterNumber.text = _counter.ToString();
    }

    public void AddCounter(int amount = 1)
    {
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