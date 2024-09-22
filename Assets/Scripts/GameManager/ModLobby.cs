using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;

public class ModLobby : Module
{
    public string LobbyCode { get; private set; }

    public async void CreateLobby()
    {
        _manager.LoadingScreenSceneHandler.Load();
        try
        {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(3);
            LobbyCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            Debug.Log(LobbyCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new(a, "dtls"));
            NetworkManager.Singleton.StartHost();

            _manager.InvokeOnLobbyCreated();
        }
        catch (RelayServiceException ex)
        {
            Debug.LogException(ex);
        }

        _manager.LoadingScreenSceneHandler.Unload();
    }

    public async void JoinLobby(string code)
    {
        _manager.LoadingScreenSceneHandler.Load();

        try
        {
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(code);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new(a, "dtls"));
            NetworkManager.Singleton.StartClient();

            LobbyCode = code.ToUpper();
            _manager.InvokeOnLobbyCreated();
        }
        catch (RelayServiceException ex)
        {
            Debug.LogException(ex);
            _manager.ShowError("Impossible de se connecter au lobby.");
        }

        _manager.LoadingScreenSceneHandler.Unload();
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