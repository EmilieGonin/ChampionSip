using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterNumber;
    [SerializeField] private bool _isStat;

    private int _counter = 0;
    private bool _isProtected;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
        ChallengeShield.OnActivate += ChallengeShield_OnActivate;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
        ChallengeShield.OnActivate -= ChallengeShield_OnActivate;
    }

    private void ChallengeShield_OnActivate()
    {
        _isProtected = true;
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

    public void AddCounter()
    {
        _counter++;
        _counterNumber.text = _counter.ToString();
    }

    public void RemoveCounter()
    {
        _counter--;
        if (_counter < 0) _counter = 0;
        _counterNumber.text = _counter.ToString();
    }
}