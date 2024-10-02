using TMPro;
using UnityEngine;

public class HUDOwner : MonoBehaviour
{
    [SerializeField] private TMP_Text _ownerName;
    [SerializeField] private HUDSipEffects _sipEffects;

    private void Start() => _ownerName.text = GameManager.Instance.PlayerName;
    private void Awake() => ModLobby.OnSetPlayerId += ModLobby_OnSetPlayerId;
    private void OnDestroy() => ModLobby.OnSetPlayerId -= ModLobby_OnSetPlayerId;
    private void ModLobby_OnSetPlayerId(ulong id) => _sipEffects.SetPlayerId(id);
}