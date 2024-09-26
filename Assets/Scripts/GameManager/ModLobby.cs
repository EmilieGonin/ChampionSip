using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using System.Threading.Tasks;

public class ModLobby : Module
{
    public string LobbyCode { get; private set; }
    public bool IsHost { get; private set; }

    public async Task<bool> CreateLobby()
    {
        _manager.LoadingScreenSceneHandler.Load();
        try
        {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(3);
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
}