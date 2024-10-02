using TMPro;
using UnityEngine;

public class HUDOwner : MonoBehaviour
{
    [SerializeField] private TMP_Text _ownerName;
    [SerializeField] private HUDSipEffects _sipEffects;

    private void Start()
    {
        _ownerName.text = GameManager.Instance.PlayerName;
        _sipEffects.SetPlayerId(GameManager.Instance.PlayerId);
    }
}