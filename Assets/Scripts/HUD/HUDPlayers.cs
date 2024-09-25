using TMPro;
using UnityEngine;

public class HUDPlayers : MonoBehaviour
{
    [SerializeField] private TMP_Text _ownerName;
    [SerializeField] private TMP_Text _friendName;

    private void Awake()
    {
        PlayerNetwork.OnGetFriendName += PlayerNetwork_OnGetFriendName;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnGetFriendName -= PlayerNetwork_OnGetFriendName;
    }

    private void Start()
    {
        _ownerName.text = GameManager.Instance.PlayerName;
    }

    private void PlayerNetwork_OnGetFriendName(string name)
    {
        _friendName.text = name;
    }
}