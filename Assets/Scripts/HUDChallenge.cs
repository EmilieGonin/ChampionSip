using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDChallenge : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _challenge;

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
        _canvasGroup.alpha = 1;
        _button.interactable = true;
        _challenge.text = $"New challenge";
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        _canvasGroup.alpha = 0;
        _button.interactable = false;
    }
}