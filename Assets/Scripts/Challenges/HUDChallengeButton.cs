using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDChallengeButton : MonoBehaviour
{
    [Header("Button Appearence")]
    [SerializeField] private Image _buttonIcon;
    [SerializeField] private Sprite _buttonDefaultIcon;
    [SerializeField] private Sprite _buttonCompleteIcon;

    [Header("Timer Appearence")]
    [SerializeField] private Image _timerImage;
    [SerializeField] private Color _timerColor;
    [SerializeField] private TMP_Text _timerText;

    private bool _isAvailable = true;

    public static event Action OnChallengeSelect;
    public static event Action OnChallengeCompleted;

    private void Awake()
    {
        PlayerNetwork.OnChallengeSelect += PlayerNetwork_OnChallengeSelect;
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeSelect -= PlayerNetwork_OnChallengeSelect;
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
    }

    private void PlayerNetwork_OnChallengeSelect()
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

    public void OnClick()
    {
        if (_isAvailable) OnChallengeSelect?.Invoke();
        else OnChallengeCompleted?.Invoke();
    }
}