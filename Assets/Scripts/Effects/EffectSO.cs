using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class EffectSO : ScriptableObject
{
    public static event Action<EffectSO> OnActivate;
    public static event Action<EffectSO> OnDeactivate;
    public static event Action<EffectSO> OnInflict;

    public string Name;
    public string Description;
    public Sprite Icon;
    public int Price;
    public int Timer;

    public virtual void Activate() => OnActivate?.Invoke(this);
    public virtual void Deactivate() => OnDeactivate?.Invoke(this);
    public virtual void Inflict() => OnInflict?.Invoke(this);
}