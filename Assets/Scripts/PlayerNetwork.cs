using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public static event Action<string> OnChallengeSelect;
    public static event Action<bool> OnChallengeCompleted; // true = victory
    public static event Action OnSipTransfer;
    public static event Action OnDoubleSip;

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

        if (effect is SipTransfer)
        {
            if (IsHost) SipTransferClientRpc();
            else SipTransferServerRpc();
        }
        else if (effect is DoubleSip)
        {
            if (IsHost) DoubleSipClientRpc();
            else DoubleSipServerRpc();
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
    [ServerRpc] private void SipTransferServerRpc() => OnSipTransfer?.Invoke();
    [ServerRpc] private void DoubleSipServerRpc() => OnDoubleSip?.Invoke();

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
    private void SipTransferClientRpc()
    {
        if (IsHost) return;
        OnSipTransfer?.Invoke();
    }

    [ClientRpc]
    private void DoubleSipClientRpc()
    {
        if (IsHost) return;
        OnDoubleSip?.Invoke();
    }
}