using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] protected TMP_Text _timer;

    public IEnumerator Launch(int minutes)
    {
        yield return SetTimer(minutes);
        OnTimerEnd();
    }

    public virtual void OnTimerEnd() { }

    private IEnumerator SetTimer(int minutes)
    {
        TimeSpan time = new(0, minutes, 0);
        TimeSpan tick = new(0, 0, 1);

        while (time.TotalSeconds > 0)
        {
            //Debug.Log(time.ToString());
            time = time.Subtract(tick);
            _timer.text = time.ToString();
            yield return new WaitForSeconds(1);
        }

        yield return null;
    }
}