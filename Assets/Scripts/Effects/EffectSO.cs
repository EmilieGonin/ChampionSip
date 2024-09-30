using System;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public static event Action<EffectSO, ulong> OnActivate;
    public static event Action<EffectSO> OnDeactivate;
    public static event Action<EffectSO> OnInflict;

    public string Name;
    public string Description;
    public Sprite Icon;
    public int Price;
    public int Timer;
    public bool IsInflicted;

    public virtual void Activate(ulong id) => OnActivate?.Invoke(this, id);
    public virtual void Deactivate() => OnDeactivate?.Invoke(this);
    public virtual void Inflict() => OnInflict?.Invoke(this);
    public virtual bool CanBuy() => Price <= GameManager.Instance.Currencies[Currency.Golds];
}