using System;
using UnityEngine;

public class SelectChallenge : MonoBehaviour
{
    public static event Action OnChallengeSelect;

    public void Select() => OnChallengeSelect?.Invoke();
}