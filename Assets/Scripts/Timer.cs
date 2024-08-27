using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] protected TMP_Text _timer;
    protected TimeSpan _time;
    protected IEnumerator _timerCoroutine;

    public IEnumerator Launch(int minutes)
    {
        yield return _timerCoroutine = SetTimer(minutes);
        OnTimerEnd();
    }

    public virtual void OnTimerEnd() { }

    private IEnumerator SetTimer(int minutes)
    {
        _time = new(0, minutes, 0);
        TimeSpan tick = new(0, 0, 1);

        while (_time.TotalSeconds > 0)
        {
            //Debug.Log(time.ToString());
            _time = _time.Subtract(tick);
            _timer.text = _time.ToString();
            yield return new WaitForSeconds(1);
        }

        yield return null;
    }
}