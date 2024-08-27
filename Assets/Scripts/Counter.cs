using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterNumber;
    [SerializeField] private bool _isStat;

    private int _counter = 0;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        if (_isStat || victory) return;
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