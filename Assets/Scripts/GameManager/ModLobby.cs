using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public struct PlayerData
{
    public string Name;
    public Dictionary<Currency, int> Currencies;
}

public class ModLobby : Module
{
    public static event Action<ulong, string, int, int, int, int> OnNewPlayer;
    public static event Action<ulong> OnSetPlayerId;

    public string LobbyCode { get; private set; }
    public bool IsHost { get; private set; }
    public Dictionary<ulong, PlayerData> Players { get; private set; } = new();
    public ulong PlayerId {  get; private set; } = ulong.MaxValue;

    private void Awake()
    {
        PlayerNetwork.OnNewPlayer += PlayerNetwork_OnNewPlayer;
        PlayerNetwork.OnPlayerDisconnect += PlayerNetwork_OnPlayerDisconnect;
        PlayerNetwork.OnCurrencyUpdate += PlayerNetwork_OnCurrencyUpdate;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnNewPlayer -= PlayerNetwork_OnNewPlayer;
        PlayerNetwork.OnPlayerDisconnect -= PlayerNetwork_OnPlayerDisconnect;
        PlayerNetwork.OnCurrencyUpdate -= PlayerNetwork_OnCurrencyUpdate;
    }

    public async Task<bool> CreateLobby()
    {
        _manager.LoadingScreenSceneHandler.Load();
        try
        {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(20);
            LobbyCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            Debug.Log(LobbyCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new(a, "dtls"));
            NetworkManager.Singleton.StartHost();

            IsHost = true;
            return true;
        }
        catch (RelayServiceException ex)
        {
            Debug.LogException(ex);
            return false;
        }
        finally
        {
            _manager.LoadingScreenSceneHandler.Unload();
        }
    }

    public async Task<bool> JoinLobby(string code)
    {
        _manager.LoadingScreenSceneHandler.Load();

        try
        {
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(code);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new(a, "dtls"));
            NetworkManager.Singleton.StartClient();

            LobbyCode = code.ToUpper();
            IsHost = false;
            return true;
        }
        catch (RelayServiceException ex)
        {
            Debug.LogException(ex);
            _manager.ShowError("Impossible de se connecter au lobby.");
            _manager.LoadingScreenSceneHandler.Unload();
            return false;
        }
    }

    public async void Reconnect(bool isHost)
    {
        _manager.LoadingScreenSceneHandler.Load();
        Debug.Log("Reconnecting to host");

        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(LobbyCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new(allocation, "dtls"));
            if (isHost) NetworkManager.Singleton.StartHost();
            else NetworkManager.Singleton.StartClient();
            Debug.Log("Successfully reconnected to the Relay server.");
        }
        catch (RelayServiceException ex)
        {
            Debug.LogException(ex);
            _manager.ShowError("Failed to reconnect to the lobby.");
        }

        _manager.LoadingScreenSceneHandler.Unload();
    }

    private void PlayerNetwork_OnNewPlayer(ulong id, string name, int sips, int sipsToDrink, int shots, int golds)
    {
        if (Players.ContainsKey(id) || PlayerId == id) return;
        Debug.Log($"Added player {name} ({id})");

        Players[id] = new()
        {
            Name = name,
            Currencies = new()
            {
                { Currency.Sips, sips },
                { Currency.SipsToDrink, sipsToDrink },
                { Currency.Shots, shots },
                { Currency.Golds, golds }
            }
        };

        OnNewPlayer?.Invoke(id, name, sips, sipsToDrink, shots, golds);
    }

    private void PlayerNetwork_OnPlayerDisconnect(ulong id)
    {
        Players.Remove(id);
    }

    public void SetPlayerId(ulong id)
    {
        PlayerId = id;
        OnSetPlayerId?.Invoke(id);
    }

    private void PlayerNetwork_OnCurrencyUpdate(ulong id, Currency currency, int amount)
    {
        Players[id].Currencies[currency] = amount;
    }

    public string CheckNewPlayerName(string name)
    {
        if (!NameExists(name)) return name;

        string newName;

        do newName = _manager.ChooseRandomName();
        while (NameExists(newName));

        return newName;
    }

    private bool NameExists(string name)
    {
        if (_manager.PlayerName == name) return true;

        foreach (var player in Players)
        {
            if (player.Value.Name == name) return true;
        }

        return false;
    }
}