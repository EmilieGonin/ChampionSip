using TMPro;
using UnityEngine;

public class HUDPlayers : MonoBehaviour
{
    [SerializeField] private TMP_Text _ownerName;
    [SerializeField] private TMP_Text _friendName;
    [SerializeField] private TMP_Text _friendGolds;

    private void Awake()
    {
        PlayerNetwork.OnGetFriendName += PlayerNetwork_OnGetFriendName;
        PlayerNetwork.OnCurrencyUpdate += PlayerNetwork_OnCurrencyUpdate;
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

    private void PlayerNetwork_OnCurrencyUpdate(Currency currency, int amount)
    {
        if (currency != Currency.Golds) return;
        _friendGolds.text = amount.ToString();
    }
}