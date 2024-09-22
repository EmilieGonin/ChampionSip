using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModEffects : Module
{
    public List<EffectSO> Effects { get; private set; } = new();
    public List<EffectSO> CurrentEffects { get; private set; } = new();

    public EffectSO GetEffectByName(string name) => Effects.Find(x => x.Name == name);
    public bool HasEffect<T>() where T : EffectSO => CurrentEffects.OfType<T>().Any();

    private void Awake()
    {
        Effects = Resources.LoadAll<EffectSO>("SO/Effects").OrderBy(x => x.Price).ToList();

        EffectSO.OnActivate += EffectSO_OnActivate;
        EffectSO.OnDeactivate += EffectSO_OnDeactivate;
        EffectSO.OnInflict += EffectSO_OnInflict;
    }

    private void OnDestroy()
    {
        EffectSO.OnActivate -= EffectSO_OnActivate;
        EffectSO.OnDeactivate -= EffectSO_OnDeactivate;
        EffectSO.OnInflict -= EffectSO_OnInflict;
    }

    private void EffectSO_OnActivate(EffectSO effect)
    {
        if (!effect.IsInflicted) CurrentEffects.Add(effect);
        _manager.RemoveCurrency(Currency.Golds, effect.Price);
        _manager.ShowNotification($"Vous avez activé l'effet <b>{effect.Name}</b> !");
    }

    private void EffectSO_OnDeactivate(EffectSO effect)
    {
        if (CurrentEffects.Contains(effect)) CurrentEffects.Remove(effect);
    }

    private void EffectSO_OnInflict(EffectSO effect)
    {
        if (effect.IsInflicted) CurrentEffects.Add(effect);
        _manager.ShowNotification($"On vous a infligé l'effet <b>{effect.Name}</b> !");
    }
}