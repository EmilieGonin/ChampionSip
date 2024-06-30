using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterNumber;

    private int _counter;

    private void Awake()
    {
        _counter = 0;
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