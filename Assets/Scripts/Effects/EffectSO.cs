using System;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public static event Action<EffectSO, ulong> OnActivate;
    public static event Action<EffectSO, ulong> OnDeactivate;
    public static event Action<EffectSO, ulong> OnInflict;

    public string Name;
    public string Description;
    public Sprite Icon;
    public int Price;
    public int Timer;
    public bool IsInflicted;

    public virtual void Activate(ulong id) => OnActivate?.Invoke(this, id);
    public virtual void Deactivate(ulong id) => OnDeactivate?.Invoke(this, id);
    public virtual void Inflict(ulong id) => OnInflict?.Invoke(this, id);
    public virtual bool CanBuy() => Price <= GameManager.Instance.Currencies[Currency.Golds];
}