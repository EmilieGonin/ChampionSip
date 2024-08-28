using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public static event Action<string> OnChallengeSelect;
    public static event Action<bool> OnChallengeCompleted; // true = victory

    public override void OnNetworkSpawn()
    {
        //Debug.Log("Player created");
        DontDestroyOnLoad(gameObject);

        HUDChallengeButton.OnChallengeSelect += HUDChallengeButton_OnChallengeSelect;
        HUDChallengeButton.OnChallengeCompleted += HUDChallengeButton_OnChallengeCompleted;
        EffectSO.OnActivate += EffectSO_OnActivate;
    }

    private void EffectSO_OnActivate(EffectSO effect)
    {
        if (!IsOwner) return;

        if (effect is DoubleSip or BottomsUp or SipTransfer)
        {
            if (IsHost) InflictEffectClientRpc(effect.Name);
            else InflictEffectServerRpc(effect.Name);
        }
    }

    public override void OnNetworkDespawn()
    {
        HUDChallengeButton.OnChallengeSelect -= HUDChallengeButton_OnChallengeSelect;
        HUDChallengeButton.OnChallengeCompleted -= HUDChallengeButton_OnChallengeCompleted;
        EffectSO.OnActivate -= EffectSO_OnActivate;
    }

    private void HUDChallengeButton_OnChallengeSelect(string challenge)
    {
        if (!IsOwner) return;
        if (IsHost) SelectChallengeClientRpc(challenge);
        else SelectChallengeServerRpc(challenge);
        OnChallengeSelect?.Invoke(challenge);
    }

    private void HUDChallengeButton_OnChallengeCompleted()
    {
        if (!IsOwner) return;
        if (IsHost) CompleteChallengeClientRpc();
        else CompleteChallengeServerRpc();
        OnChallengeCompleted?.Invoke(true);
    }

    [ServerRpc] private void SelectChallengeServerRpc(string challenge) => OnChallengeSelect?.Invoke(challenge);
    [ServerRpc] private void CompleteChallengeServerRpc() => OnChallengeCompleted?.Invoke(false);
    [ServerRpc] private void InflictEffectServerRpc(string effect) => GameManager.Instance.GetEffectByName(effect).Inflict();

    [ClientRpc]
    private void SelectChallengeClientRpc(string challenge)
    {
        if (IsHost) return;
        OnChallengeSelect?.Invoke(challenge);
    }

    [ClientRpc]
    private void CompleteChallengeClientRpc()
    {
        if (IsHost) return;
        OnChallengeCompleted?.Invoke(false);
    }

    [ClientRpc]
    private void InflictEffectClientRpc(string effect)
    {
        if (IsHost) return;
        GameManager.Instance.GetEffectByName(effect).Inflict();
    }
}