using System;
using System.Collections.Generic;
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
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeSelect -= PlayerNetwork_OnChallengeSelect;
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
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

    public void OnClick()
    {
        if (_isAvailable)
        {
            int rand = UnityEngine.Random.Range(0, GameManager.Instance.Challenges.Count);
            OnChallengeSelect?.Invoke(GameManager.Instance.Challenges[rand]);
        }
        else OnChallengeCompleted?.Invoke();
    }
}