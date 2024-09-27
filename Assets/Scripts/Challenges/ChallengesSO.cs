using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum ChallengeCategory
{
    VideoGame,
    BoardGame,
    Sport,
    Social,
    Memory
}

[CreateAssetMenu(fileName = "Challenges", menuName = "Game/Challenges")]
public class ChallengesSO : ScriptableObject
{
    public SerializedDictionary<string, ChallengeCategory> Challenges;
}