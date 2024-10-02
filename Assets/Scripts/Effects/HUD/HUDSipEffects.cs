using UnityEngine;
using UnityEngine.UI;

public class HUDSipEffects : Timer
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _shieldIcon;
    [SerializeField] private Sprite _doubleSipIcon;
    [SerializeField] private CanvasGroup _canvasGroup;

    private EffectSO _currentEffect;
    private ulong _playerId;

    private void Awake()
    {
        EffectSO.OnActivate += EffectSO_OnActivate;
        EffectSO.OnInflict += EffectSO_OnInflict;
    }

    private void OnDestroy()
    {
        EffectSO.OnActivate -= EffectSO_OnActivate;
        EffectSO.OnInflict -= EffectSO_OnInflict;
    }

    public void SetPlayerId(ulong id) => _playerId = id;

    private void EffectSO_OnActivate(EffectSO effect, ulong id)
    {
        if (id != _playerId) return;

        if (effect is Shield)
        {
            ChangeEffect(effect);
            _icon.sprite = _shieldIcon;
        }
    }

    private void EffectSO_OnInflict(EffectSO effect, ulong id)
    {
        if (id != _playerId) return;

        if (effect is DoubleSip)
        {
            ChangeEffect(effect);
            _icon.sprite = _doubleSipIcon;
        }
    }

    private void ChangeEffect(EffectSO effect)
    {
        if (_time.TotalSeconds > 0) StopCoroutine(_timerCoroutine);
        _currentEffect?.Deactivate(_playerId);
        _currentEffect = effect;
        _canvasGroup.alpha = 1;
        StartCoroutine(Launch(effect.Timer));
    }

    public override void OnTimerEnd()
    {
        _canvasGroup.alpha = 0;
        _currentEffect?.Deactivate(_playerId);
    }
}