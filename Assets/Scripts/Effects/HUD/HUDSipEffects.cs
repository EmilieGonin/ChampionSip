using UnityEngine;
using UnityEngine.UI;

public class HUDSipEffects : Timer
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _shieldIcon;
    [SerializeField] private Sprite _doubleSipIcon;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EffectSO _doubleSipEffect;
    [SerializeField] private EffectSO _shieldEffect;
}