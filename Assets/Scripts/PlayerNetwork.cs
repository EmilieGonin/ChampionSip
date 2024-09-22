using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public static event Action<string> OnChallengeSelect;
    public static event Action<bool> OnChallengeCompleted; // true = victory

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DontDestroyOnLoad(gameObject);

        HUDChallengeButton.OnChallengeSelect += HUDChallengeButton_OnChallengeSelect;
        HUDChallengeButton.OnChallengeCompleted += HUDChallengeButton_OnChallengeCompleted;
        EffectSO.OnActivate += EffectSO_OnActivate;

        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
    }

    private void Singleton_OnClientDisconnectCallback(ulong id)
    {
        if (!IsOwner || OwnerClientId == id) return;
        GameManager.Instance.ShowError("Un joueur s'est déconnecté !");
    }

    private void Singleton_OnClientConnectedCallback(ulong id)
    {
        if (!IsOwner) return;

        if (OwnerClientId == id)
        {
            //Reconnect
        }
        else GameManager.Instance.ShowNotification("Un joueur s'est connecté !");
    }

    private void EffectSO_OnActivate(EffectSO effect)
    {
        if (!IsOwner) return;

        if (effect.IsInflicted)
        {
            if (IsHost) InflictEffectClientRpc(effect.Name);
            else InflictEffectServerRpc(effect.Name);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        HUDChallengeButton.OnChallengeSelect -= HUDChallengeButton_OnChallengeSelect;
        HUDChallengeButton.OnChallengeCompleted -= HUDChallengeButton_OnChallengeCompleted;
        EffectSO.OnActivate -= EffectSO_OnActivate;

        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectCallback;
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

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!IsOwner) return;

        if (!pauseStatus)
        {
            GameManager.Instance.ShowNotification("Reconnecting");
            GameManager.Instance.Mod<ModLobby>().Reconnect(IsHost);
        }
    }

    private void Update()
    {
        // Appuyer sur la touche "P" pour simuler la mise en veille
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Simulating app going into background...");
            OnApplicationPause(true);
        }

        // Appuyer sur la touche "R" pour simuler la reprise de l'application
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Simulating app resuming...");
            OnApplicationPause(false);
        }
    }
}