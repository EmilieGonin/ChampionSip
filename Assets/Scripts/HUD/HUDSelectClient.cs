using System;
using UnityEngine;

public class HUDSelectClient : HUDPlayers
{
    public static event Action OnSelectPlayerCancel;

    [SerializeField] private SceneHandler _sceneHandler;

    private void Awake()
    {
        EffectSO.OnActivate += EffectSO_OnActivate;

        foreach (var player in GameManager.Instance.Players)
        {
            AddPlayer(
                player.Key,
                player.Value.Name,
                player.Value.Currencies[Currency.Sips],
                player.Value.Currencies[Currency.SipsToDrink],
                player.Value.Currencies[Currency.Shots],
                player.Value.Currencies[Currency.Golds]);
        }
    }

    private void OnDestroy() => EffectSO.OnActivate -= EffectSO_OnActivate;

    private void EffectSO_OnActivate(EffectSO effect, ulong id) => _sceneHandler.Unload();
    public void Click() => OnSelectPlayerCancel?.Invoke();
}