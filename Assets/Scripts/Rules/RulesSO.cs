using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum Category
{
    General, Fortnite, DarkAndDarker
}

[CreateAssetMenu(fileName = "Rules", menuName = "Game/Rules")]
public class RulesSO : ScriptableObject
{
    public SerializedDictionary<string, Category> Rules;
}