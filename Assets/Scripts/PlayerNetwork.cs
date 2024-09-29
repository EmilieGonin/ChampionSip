using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public static event Action<string> OnChallengeSelect;
    public static event Action<bool> OnChallengeCompleted; // true = victory
    public static event Action<ulong, Currency, int> OnCurrencyUpdate;
    public static event Action<ulong, string, int, int> OnNewPlayer;
    public static event Action<ulong> OnPlayerDisconnect;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DontDestroyOnLoad(gameObject);

        HUDChallengeButton.OnChallengeSelect += HUDChallengeButton_OnChallengeSelect;
        HUDVictoryButton.OnChallengeCompleted += HUDVictoryButton_OnChallengeCompleted;
        EffectSO.OnActivate += EffectSO_OnActivate;

        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;

        ModEconomy.OnCurrencyUpdate += ModEconomy_OnCurrencyUpdate;

        if (!IsOwner) return;
        GameManager.Instance.SetPlayerId(OwnerClientId);

        if (IsHost) return;
        SendPlayerDatasServerRpc(
            GameManager.Instance.PlayerName,
            GameManager.Instance.Currencies[Currency.Sips],
            GameManager.Instance.Currencies[Currency.Shots],
            OwnerClientId);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        HUDChallengeButton.OnChallengeSelect -= HUDChallengeButton_OnChallengeSelect;
        HUDVictoryButton.OnChallengeCompleted -= HUDVictoryButton_OnChallengeCompleted;
        EffectSO.OnActivate -= EffectSO_OnActivate;

        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectCallback;

        ModEconomy.OnCurrencyUpdate -= ModEconomy_OnCurrencyUpdate;
    }

    private void Singleton_OnClientDisconnectCallback(ulong id)
    {
        if (!IsOwner || OwnerClientId == id) return;
        GameManager.Instance.ShowError("Un joueur s'est déconnecté !");
        OnPlayerDisconnect?.Invoke(id);
    }

    private void Singleton_OnClientConnectedCallback(ulong id)
    {
        if (!IsOwner) return;

        if (OwnerClientId != id)
        {
            GameManager.Instance.ShowNotification("Un joueur s'est connecté !");
        }

        if (IsHost && OwnerClientId != id)
        {
            SendPlayerDatasClientRpc(
            GameManager.Instance.PlayerName,
            GameManager.Instance.Currencies[Currency.Sips],
            GameManager.Instance.Currencies[Currency.Shots],
            OwnerClientId);

            foreach (var player in GameManager.Instance.Players)
            {
                SendPlayerDatasClientRpc(
                    player.Value.Name,
                    player.Value.Currencies[Currency.Sips],
                    player.Value.Currencies[Currency.Shots],
                    player.Key);
            }
        }
    }

    private void EffectSO_OnActivate(EffectSO effect, ulong id)
    {
        if (!IsOwner) return;

        if (effect.IsInflicted)
        {
            if (IsHost) InflictEffectClientRpc(effect.Name, id);
            else InflictEffectServerRpc(effect.Name, id);
        }
    }

    private void HUDChallengeButton_OnChallengeSelect(string challenge)
    {
        if (!IsOwner) return;
        if (IsHost) SelectChallengeClientRpc(challenge);
        else SelectChallengeServerRpc(challenge);
        OnChallengeSelect?.Invoke(challenge);
    }

    private void HUDVictoryButton_OnChallengeCompleted()
    {
        if (!IsOwner) return;
        if (IsHost) CompleteChallengeClientRpc(OwnerClientId);
        else CompleteChallengeServerRpc(OwnerClientId);
        OnChallengeCompleted?.Invoke(true);
    }

    private void ModEconomy_OnCurrencyUpdate(Currency currency, int amount)
    {
        if (!IsOwner) return;
        if (IsHost) UpdateCurrencyClientRpc(OwnerClientId, currency, amount);
        else UpdateCurrencyServerRpc(OwnerClientId, currency, amount);
    }

    // ServerRpc : Receive only by host - Used by clients
    [ServerRpc] private void SelectChallengeServerRpc(string challenge)
    {
        OnChallengeSelect?.Invoke(challenge);
        SelectChallengeClientRpc(challenge);
    }

    [ServerRpc] private void CompleteChallengeServerRpc(ulong clientId)
    {
        OnChallengeCompleted?.Invoke(false);
        CompleteChallengeClientRpc(clientId);
    }

    [ServerRpc] private void InflictEffectServerRpc(string effect, ulong id)
    {
        if (GameManager.Instance.PlayerId == id)
        {
            GameManager.Instance.GetEffectByName(effect).Inflict();
        }
        else InflictEffectClientRpc(effect, id);
    }

    [ServerRpc] private void UpdateCurrencyServerRpc(ulong id, Currency currency, int amount)
    {
        OnCurrencyUpdate?.Invoke(id, currency, amount);
        UpdateCurrencyClientRpc(id, currency, amount);
    }

    [ServerRpc] private void SendPlayerDatasServerRpc(string name, int sips, int shots, ulong id)
    {
        Debug.Log($"[{OwnerClientId}] Server RPC - Receive datas from {name} ({id})");
        OnNewPlayer?.Invoke(id, name, sips, shots);
    }

    // ClientRpc : Receive by everyone - Used by host
    [ClientRpc]
    private void SelectChallengeClientRpc(string challenge)
    {
        if (IsHost) return;
        OnChallengeSelect?.Invoke(challenge);
    }

    [ClientRpc]
    private void CompleteChallengeClientRpc(ulong clientId)
    {
        if (IsHost || GameManager.Instance.PlayerId == clientId) return;
        OnChallengeCompleted?.Invoke(false);
    }

    [ClientRpc]
    private void InflictEffectClientRpc(string effect, ulong id)
    {
        if (IsHost || GameManager.Instance.PlayerId != id) return;
        GameManager.Instance.GetEffectByName(effect).Inflict();
    }

    [ClientRpc]
    private void UpdateCurrencyClientRpc(ulong id, Currency currency, int amount)
    {
        if (IsHost || GameManager.Instance.PlayerId == id) return;
        OnCurrencyUpdate?.Invoke(id,currency, amount);
    }

    [ClientRpc]
    private void SendPlayerDatasClientRpc(string name, int sips, int shots, ulong id)
    {
        if (IsHost) return;
        Debug.Log($"[{OwnerClientId}] Client RPC - Receive datas of {name} ({id}) ");
        //if (IsHost && OwnerClientId == id) return;
        OnNewPlayer?.Invoke(id, name, sips, shots);
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
}