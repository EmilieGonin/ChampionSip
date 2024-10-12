using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUDOwner : MonoBehaviour
{
    [SerializeField] private TMP_Text _ownerName;
    [SerializeField] private HUDSipEffects _sipEffects;
    [SerializeField] private GameObject _hostIcon;

    private void Awake() => PlayerNetwork.OnChangeName += PlayerNetwork_OnChangeName;

    private void OnDestroy()
    {
        PlayerNetwork.OnChangeName -= PlayerNetwork_OnChangeName;
        ModLobby.OnSetPlayerId -= SetPlayerId;
    }

    private void Start()
    {
        _ownerName.text = GameManager.Instance.PlayerName;

        if (GameManager.Instance.PlayerId == ulong.MaxValue)
        {
            ModLobby.OnSetPlayerId += SetPlayerId;
        }
        else SetPlayerId(GameManager.Instance.PlayerId);
    }

    private void SetPlayerId(ulong id)
    {
        _sipEffects.SetPlayerId(id);
        _hostIcon.SetActive(id == 0);
    }

    private void PlayerNetwork_OnChangeName(string name) => _ownerName.text = name;
}