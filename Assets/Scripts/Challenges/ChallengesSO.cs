using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenges", menuName = "Game/Challenges")]
public class ChallengesSO : ScriptableObject
{
    public List<string> Challenges;
}