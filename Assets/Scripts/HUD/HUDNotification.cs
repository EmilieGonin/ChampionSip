using TMPro;
using UnityEngine;

public class HUDNotification : MonoBehaviour
{
    [SerializeField] private TMP_Text _notification;

    private void Awake()
    {
        _notification.text = GameManager.Instance.NotificationMessage;
    }
}