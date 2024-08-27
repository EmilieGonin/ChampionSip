using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Netcode;

public class ModAccount : Module
{
    public Lobby GameLobby {  get; private set; }

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

    async public void CreateLobby()
    {
        _manager.SceneHandler.Load();
        string lobbyName = "new lobby";
        int maxPlayers = 4;
        CreateLobbyOptions options = new();
        options.IsPrivate = true;
        options.Player = new Player(
            id: AuthenticationService.Instance.PlayerId,
            data: new Dictionary<string, PlayerDataObject>()
            {
                {
                    "ExampleMemberPlayerData", new PlayerDataObject(
                        visibility: PlayerDataObject.VisibilityOptions.Member, // Visible only to members of the lobby.
                        value: "ExampleMemberPlayerData")
                }
            });

        GameLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        NetworkManager.Singleton.StartHost();
        _manager.SceneHandler.Unload();
        _manager.InvokeOnLobbyCreated();
        //Debug.Log(GameLobby.LobbyCode);
    }

    //private void StartServer()
    //{
    //    NetworkManager.Singleton.StartServer();
    //}

    async public void JoinLobby(string code)
    {
        _manager.SceneHandler.Load();

        try
        {
            GameLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            NetworkManager.Singleton.StartClient();
            _manager.InvokeOnLobbyCreated();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

        _manager.SceneHandler.Unload();
    }
}