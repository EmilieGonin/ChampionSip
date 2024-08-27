using UnityEngine;
using UnityEngine.UI;

public class ChallengeTimer : Timer
{
    [SerializeField] private Button _timerButton;
    [SerializeField] private Image _timerIcon;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += StartTimer;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= StartTimer;
    }

    public void StartTimer(bool victory)
    {
        _timerButton.interactable = false;
        _timerIcon.color = _timerButton.colors.disabledColor;
        StartCoroutine(Launch(5));
    }

    public override void OnTimerEnd()
    {
        _timer.text = "Défi disponible";
        _timerButton.interactable = true;
        _timerIcon.color = Color.white;
    }
}