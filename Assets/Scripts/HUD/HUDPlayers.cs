using System.Collections.Generic;
using UnityEngine;

public class HUDPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _joueurPrefab;
    [SerializeField] private GameObject _joueurSpacePrefab;

    private Dictionary<ulong, GameObject> _players = new();
    private Dictionary<ulong, GameObject> _playersSpaces = new();

    private void Awake()
    {
        ModLobby.OnNewPlayer += ModLobby_OnNewPlayer;
        PlayerNetwork.OnPlayerDisconnect += PlayerNetwork_OnPlayerDisconnect;
    }

    private void OnDestroy()
    {
        ModLobby.OnNewPlayer -= ModLobby_OnNewPlayer;
        PlayerNetwork.OnPlayerDisconnect -= PlayerNetwork_OnPlayerDisconnect;
    }
    private void ModLobby_OnNewPlayer(ulong id, string name, int sips, int shots)
    {
        GameObject go = Instantiate(_joueurPrefab, transform);
        _players[id] = go;
        go.GetComponent<HUDPlayer>().Init(id);

        go = Instantiate(_joueurSpacePrefab, transform);
        _playersSpaces[id] = go;
    }

    private void PlayerNetwork_OnPlayerDisconnect(ulong id)
    {
        Destroy(_players[id]);
        Destroy(_playersSpaces[id]);
        _players.Remove(id);
        _playersSpaces.Remove(id);
    }
}