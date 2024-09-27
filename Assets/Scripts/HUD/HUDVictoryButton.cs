using System;
using UnityEngine;

public class HUDVictoryButton : MonoBehaviour
{
    public static event Action OnChallengeCompleted;

    public void OnClick() => OnChallengeCompleted?.Invoke();
}