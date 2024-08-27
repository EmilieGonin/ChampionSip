using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private Button _timerButton;
    [SerializeField] private Image _timerIcon;

    private void Awake()
    {
        PlayerNetwork.OnChallengeSelect += StartTimer;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeSelect -= StartTimer;
    }

    public void StartTimer() => StartCoroutine(Timer());

    private IEnumerator Timer()
    {
        _timerButton.interactable = false;
        _timerIcon.color = _timerButton.colors.disabledColor;

        TimeSpan time = new(0, 5, 0);
        TimeSpan tick = new(0, 0, 1);

        while (time.TotalSeconds > 0)
        {
            //Debug.Log(time.ToString());
            time = time.Subtract(tick);
            _timer.text = time.ToString();
            yield return new WaitForSeconds(1);
        }

        _timer.text = "Défi disponible";
        _timerButton.interactable = true;
        _timerIcon.color = Color.white;
        yield return null;
    }
}