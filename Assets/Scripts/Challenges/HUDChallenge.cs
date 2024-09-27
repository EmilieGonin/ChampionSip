using TMPro;
using UnityEngine;

public class HUDChallenge : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
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

    private void PlayerNetwork_OnChallengeSelect(string challenge)
    {
        _canvasGroup.alpha = 1;
        _challenge.text = challenge;
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        _canvasGroup.alpha = 0;
    }
}