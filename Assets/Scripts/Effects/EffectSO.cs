using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class EffectSO : ScriptableObject
{
    public static event Action<int> OnBuy;
    public static event Action<EffectSO> OnActivate;

    public string Name;
    public string Description;
    public Image Icon;
    public int Price;
    public int Timer;

    public virtual void Activate()
    {
        OnBuy?.Invoke(Price);
        OnActivate?.Invoke(this);
    }
}