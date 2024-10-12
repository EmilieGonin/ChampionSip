using TMPro;
using UnityEngine;

public class HUDOwner : MonoBehaviour
{
    [SerializeField] private TMP_Text _ownerName;
    [SerializeField] private HUDSipEffects _sipEffects;
    [SerializeField] private GameObject _hostIcon;

    private void Start()
    {
        _ownerName.text = GameManager.Instance.PlayerName;

        if (GameManager.Instance.PlayerId == ulong.MaxValue)
        {
            ModLobby.OnSetPlayerId += SetPlayerId;
        }
        else SetPlayerId(GameManager.Instance.PlayerId);
    }
    private void OnDestroy() => ModLobby.OnSetPlayerId -= SetPlayerId;

    private void SetPlayerId(ulong id)
    {
        _sipEffects.SetPlayerId(id);
        _hostIcon.SetActive(id == 0);
    }
}