using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public abstract class EffectSO : ScriptableObject
{
    public string Name;
    public string Description;
    public Image Icon;
    public int Price;
    public int Timer;
    public abstract void Activate();
}