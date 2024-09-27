using UnityEngine;
using UnityEngine.UI;

public class ChallengeTimer : Timer
{
    [SerializeField] private Button _timerButton;
    [SerializeField] private Image _timerIcon;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += StartTimer;
        PlayerNetwork.OnChallengeSelect += PlayerNetwork_OnChallengeSelect;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= StartTimer;
        PlayerNetwork.OnChallengeSelect -= PlayerNetwork_OnChallengeSelect;
    }

    private void PlayerNetwork_OnChallengeSelect(string obj)
    {
        if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
        _timerButton.interactable = false;
        _timerIcon.color = _timerButton.colors.disabledColor;
        //_timerIcon.color = Color.white;
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