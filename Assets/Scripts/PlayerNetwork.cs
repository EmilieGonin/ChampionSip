using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public static event Action OnChallengeSelect;

    private NetworkVariable<int> _challenge = new();

    public override void OnNetworkSpawn()
    {
        //Debug.Log("Player created");
        DontDestroyOnLoad(gameObject);

        SelectChallenge.OnChallengeSelect += SelectChallenge_OnChallengeSelect;
    }

    public override void OnNetworkDespawn()
    {
        SelectChallenge.OnChallengeSelect -= SelectChallenge_OnChallengeSelect;
    }

    private void SelectChallenge_OnChallengeSelect()
    {
        if (!IsOwner) return;

        if (IsHost)
        {
            SelectChallengeClientRpc();
            return;
        }

        SelectChallengeServerRpc();
        OnChallengeSelect?.Invoke();
    }

    [ServerRpc]
    private void SelectChallengeServerRpc()
    {
        Debug.Log("New challenge from client");
        OnChallengeSelect?.Invoke();
    }

    [ClientRpc]
    private void SelectChallengeClientRpc()
    {
        if (IsHost) return;
        Debug.Log("New challenge from host");
        OnChallengeSelect?.Invoke();
    }

    //private void Update()
    //{
    //    Debug.Log($"{OwnerClientId} : {_challenge.Value}");
    //    if (!IsOwner) return;
    //}
}