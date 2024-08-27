using UnityEngine;
using UnityEngine.UI;

public class HUDSipEffects : Timer
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _shieldIcon;
    [SerializeField] private Sprite _doubleSipIcon;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EffectSO _doubleSipEffect;

    private void Awake()
    {
        EffectSO.OnActivate += EffectSO_OnActivate;
        PlayerNetwork.OnDoubleSip += PlayerNetwork_OnDoubleSip;
    }

    private void OnDestroy()
    {
        EffectSO.OnActivate -= EffectSO_OnActivate;
        PlayerNetwork.OnDoubleSip -= PlayerNetwork_OnDoubleSip;
    }

    private void EffectSO_OnActivate(EffectSO effect)
    {
        if (effect is Shield)
        {
            if (_time.TotalSeconds > 0) StopCoroutine(_timerCoroutine);
            _icon.sprite = _shieldIcon;
            _canvasGroup.alpha = 1;
            StartCoroutine(Launch(effect.Timer));
        }
    }

    private void PlayerNetwork_OnDoubleSip()
    {
        if (_time.TotalSeconds > 0) StopCoroutine(_timerCoroutine);
        _icon.sprite = _doubleSipIcon;
        _canvasGroup.alpha = 1;
        StartCoroutine(Launch(_doubleSipEffect.Timer));
    }

    public override void OnTimerEnd()
    {
        _canvasGroup.alpha = 0;
    }
}