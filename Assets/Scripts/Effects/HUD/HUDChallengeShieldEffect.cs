using UnityEngine;

public class HUDChallengeShieldEffect : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate += Show;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
        EffectSO.OnActivate -= Show;
    }

    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        if (!victory) Hide();
    }

    private void Show(EffectSO effect, ulong id)
    {
        if (effect is ChallengeShield) _canvasGroup.alpha = 1;
    }

    private void Hide() => _canvasGroup.alpha = 0;
}