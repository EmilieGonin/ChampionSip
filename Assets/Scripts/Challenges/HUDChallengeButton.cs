using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDChallengeButton : MonoBehaviour
{
    public static event Action<string> OnChallengeSelect;
    public static event Action OnChallengeCompleted;

    [Header("Button Appearence")]
    [SerializeField] private Image _buttonIcon;
    [SerializeField] private Sprite _buttonDefaultIcon;
    [SerializeField] private Sprite _buttonCompleteIcon;

    [Header("Timer Appearence")]
    [SerializeField] private Image _timerImage;
    [SerializeField] private Color _timerColor;
    [SerializeField] private TMP_Text _timerText;

    private bool _isAvailable = true;

    private void Awake()
    {
        PlayerNetwork.OnChallengeSelect += PlayerNetwork_OnChallengeSelect;
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate += EffectSO_OnActivate;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeSelect -= PlayerNetwork_OnChallengeSelect;
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate -= EffectSO_OnActivate;
    }

    private void PlayerNetwork_OnChallengeSelect(string challenge)
    {
        _isAvailable = false;
        _buttonIcon.sprite = _buttonCompleteIcon;
        _timerImage.color = _timerColor;
        _timerText.text = "Victoire !";
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        _isAvailable = true;
        _buttonIcon.sprite = _buttonDefaultIcon;
        _timerImage.color = Color.white;
    }

    private void EffectSO_OnActivate(EffectSO effect)
    {
        if (effect is RandomChallengeRelaunch) CreateNewChallenge();
    }

    public void OnClick()
    {
        if (_isAvailable) CreateNewChallenge();
        else OnChallengeCompleted?.Invoke();
    }

    private void CreateNewChallenge()
    {
        int rand = UnityEngine.Random.Range(0, GameManager.Instance.Challenges.Count);
        OnChallengeSelect?.Invoke(GameManager.Instance.Challenges[rand]);
    }
}