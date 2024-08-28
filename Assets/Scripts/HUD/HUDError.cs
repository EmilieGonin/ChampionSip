using TMPro;
using UnityEngine;

public class HUDError : MonoBehaviour
{
    [SerializeField] private TMP_Text _error;

    private void Awake()
    {
        _error.text = GameManager.Instance.ErrorMessage;
    }
}