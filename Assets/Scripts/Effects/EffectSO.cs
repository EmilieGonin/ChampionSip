using UnityEngine;
using UnityEngine.UI;

public abstract class EffectSO : ScriptableObject
{
    public string Name;
    public Image Icon;
    public abstract void Activate();
}