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

    public void OnClick()
    {
        if (_isAvailable)
        {
            _isAvailable = false;
            _buttonIcon.sprite = _buttonCompleteIcon;
            _timerImage.color = _timerColor;
            _timerText.text = "Victoire !";
            OnChallengeSelect?.Invoke();
        }
        else
        {
            _isAvailable = true;
            _buttonIcon.sprite = _buttonDefaultIcon;
            _timerImage.color = Color.white;
            OnChallengeCompleted?.Invoke();
        }
    }
}