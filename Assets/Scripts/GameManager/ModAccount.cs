using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class ModAccount : Module
{
    public string LobbyCode {  get; private set; }

    async private void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Setup authentication event handlers if desired
    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async void CreateLobby()
    {
        _manager.SceneHandler.Load();

        try
        {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(3);
            LobbyCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            Debug.Log(LobbyCode);

            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
            //    a.RelayServer.IpV4,
            //    (ushort)a.RelayServer.Port,
            //    a.AllocationIdBytes,
            //    a.Key,
            //    a.ConnectionData
            //    );

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new(a, "dtls"));
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException ex)
        {
            Debug.LogException(ex);
        }

        _manager.SceneHandler.Unload();
        _manager.InvokeOnLobbyCreated();
    }

    public async void JoinLobby(string code)
    {
        _manager.SceneHandler.Load();

        try
        {
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(code);

            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
            //    a.RelayServer.IpV4,
            //    (ushort)a.RelayServer.Port,
            //    a.AllocationIdBytes,
            //    a.Key,
            //    a.ConnectionData,
            //    a.HostConnectionData
            //    );

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new(a, "dtls"));
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException ex)
        {
            Debug.LogException(ex);
        }

        LobbyCode = code;
        _manager.SceneHandler.Unload();
        _manager.InvokeOnLobbyCreated();
    }

    //async public void CreateLobbyOld()
    //{
    //    _manager.SceneHandler.Load();
    //    string lobbyName = "new lobby";
    //    int maxPlayers = 4;
    //    CreateLobbyOptions options = new();
    //    options.IsPrivate = true;
    //    options.Player = new Player(
    //        id: AuthenticationService.Instance.PlayerId,
    //        data: new Dictionary<string, PlayerDataObject>()
    //        {
    //            {
    //                "ExampleMemberPlayerData", new PlayerDataObject(
    //                    visibility: PlayerDataObject.VisibilityOptions.Member, // Visible only to members of the lobby.
    //                    value: "ExampleMemberPlayerData")
    //            }
    //        });

    //    GameLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
    //    _manager.SceneHandler.Unload();
    //    _manager.InvokeOnLobbyCreated();
    //    Debug.Log(GameLobby.LobbyCode);
    //}

    //async public void JoinLobbyOld(string code)
    //{
    //    _manager.SceneHandler.Load();

    //    try
    //    {
    //        GameLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
    //        _manager.InvokeOnLobbyCreated();
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //    }

    //    _manager.SceneHandler.Unload();
    //}
}