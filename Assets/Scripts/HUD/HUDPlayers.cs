using System.Collections.Generic;
using UnityEngine;

public class HUDPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _joueurPrefab;
    [SerializeField] private RectTransform _content;

    private Dictionary<ulong, GameObject> _players = new();

    private void Awake()
    {
        ModLobby.OnNewPlayer += AddPlayer;
        PlayerNetwork.OnPlayerDisconnect += RemovePlayer;
    }

    private void OnDestroy()
    {
        ModLobby.OnNewPlayer -= AddPlayer;
        PlayerNetwork.OnPlayerDisconnect -= RemovePlayer;
    }

    protected void AddPlayer(ulong id, string name, int sips, int shots)
    {
        GameObject go = Instantiate(_joueurPrefab, _content);
        _players[id] = go;
        go.GetComponent<HUDPlayer>().Init(id);
    }

    private void RemovePlayer(ulong id)
    {
        Destroy(_players[id]);
        _players.Remove(id);
    }
}