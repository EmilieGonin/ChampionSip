using System;
using UnityEngine;

public class HUDButton : MonoBehaviour
{
    public static event Action OnClick;

    public void Click() => OnClick?.Invoke();
}