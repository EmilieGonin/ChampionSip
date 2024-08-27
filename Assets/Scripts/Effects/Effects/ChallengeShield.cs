using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeShield", menuName = "Game/Effects/ChallengeShield")]
public class ChallengeShield : EffectSO
{
    public static event Action OnActivate;

    public override void Activate()
    {
        base.Activate();
        OnActivate?.Invoke();
    }
}