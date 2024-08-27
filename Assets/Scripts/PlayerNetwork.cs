using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public static event Action OnChallengeSelect;
    public static event Action OnChallengeCompleted;

    public override void OnNetworkSpawn()
    {
        //Debug.Log("Player created");
        DontDestroyOnLoad(gameObject);

        HUDChallengeButton.OnChallengeSelect += HUDChallengeButton_OnChallengeSelect;
        HUDChallengeButton.OnChallengeCompleted += HUDChallengeButton_OnChallengeCompleted;
    }

    public override void OnNetworkDespawn()
    {
        HUDChallengeButton.OnChallengeSelect -= HUDChallengeButton_OnChallengeSelect;
        HUDChallengeButton.OnChallengeCompleted -= HUDChallengeButton_OnChallengeCompleted;
    }

    private void HUDChallengeButton_OnChallengeSelect()
    {
        if (!IsOwner) return;
        if (IsHost) SelectChallengeClientRpc();
        else SelectChallengeServerRpc();
        OnChallengeSelect?.Invoke();
    }

    private void HUDChallengeButton_OnChallengeCompleted()
    {
        if (!IsOwner) return;
        if (IsHost) CompleteChallengeClientRpc();
        else CompleteChallengeServerRpc();
        OnChallengeCompleted?.Invoke();
    }

    [ServerRpc]
    private void SelectChallengeServerRpc() => OnChallengeSelect?.Invoke();

    [ServerRpc]
    private void CompleteChallengeServerRpc() => OnChallengeSelect?.Invoke();

    [ClientRpc]
    private void SelectChallengeClientRpc()
    {
        if (IsHost) return;
        OnChallengeCompleted?.Invoke();
    }

    [ClientRpc]
    private void CompleteChallengeClientRpc()
    {
        if (IsHost) return;
        OnChallengeCompleted?.Invoke();
    }
}