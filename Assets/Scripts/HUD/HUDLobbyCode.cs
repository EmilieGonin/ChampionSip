using TMPro;
using UnityEngine;

public class HUDLobbyCode : MonoBehaviour
{
    [SerializeField] private TMP_Text _code;

    private void Awake()
    {
        _code.text = $"Code du lobby : {GameManager.Instance.Mod<ModLobby>().LobbyCode}";
    }
}