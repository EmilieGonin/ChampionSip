using System;
using UnityEngine;

public class Economy : MonoBehaviour
{
    public static event Action<int> OnAddGold;

    public void AddGold(int amount) => OnAddGold?.Invoke(amount);
}