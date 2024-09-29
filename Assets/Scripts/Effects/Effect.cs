using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public static event Action<EffectSO> OnEffectSelect;

    [SerializeField] private TMP_Text _effectName;
    [SerializeField] private TMP_Text _effectDescription;
    [SerializeField] private TMP_Text _effectPrice;
    [SerializeField] private Image _effectIcon;

    private EffectSO _effect;

    public void Init(EffectSO effect)
    {
        _effect = effect;
        _effectName.text = effect.Name;
        _effectDescription.text = effect.Description;
        _effectPrice.text = effect.Price.ToString();
        _effectIcon.sprite = effect.Icon;
    }

    public void Activate()
    {
        if (_effect.IsInflicted && GameManager.Instance.Players.Count > 1)
        {
            OnEffectSelect?.Invoke(_effect);
            return;
        }

        ulong id = _effect.IsInflicted ? GameManager.Instance.Players.First().Key : GameManager.Instance.PlayerId;

        _effect.Activate(id);
    }
}