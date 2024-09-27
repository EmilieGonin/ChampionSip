using TMPro;
using UnityEngine;

public class HUDOwner : MonoBehaviour
{
    [SerializeField] private TMP_Text _ownerName;

    private void Start()
    {
        _ownerName.text = GameManager.Instance.PlayerName;
    }
}