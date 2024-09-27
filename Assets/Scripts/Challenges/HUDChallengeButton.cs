using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDChallengeButton : MonoBehaviour
{
    public static event Action<string> OnChallengeSelect;

    [SerializeField] private Button _timerButton;

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
        _timerImage.enabled = false;
        _timerText.alpha = 0;
        //_buttonIcon.sprite = _buttonCompleteIcon;
        //_timerImage.color = _timerColor;
        //_timerText.text = "Victoire !";
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        _isAvailable = true;
        _timerImage.enabled = true;
        _timerText.alpha = 1;
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
    }

    private void CreateNewChallenge()
    {
        OnChallengeSelect?.Invoke(GameManager.Instance.SelectNewChallenge());
    }
}