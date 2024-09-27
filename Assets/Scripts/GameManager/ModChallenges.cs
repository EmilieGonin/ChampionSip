using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ModChallenges : Module
{
    [SerializeField] private SerializedDictionary<ChallengeCategory, string> _challengeCategoriesTitles = new();

    public Dictionary<string, ChallengeCategory> Challenges { get; private set; } = new();
    public Dictionary<ChallengeCategory, bool> ChallengeCategories { get; private set; } = new();
    public SerializedDictionary<ChallengeCategory, string> ChallengeCategoriesTitles => _challengeCategoriesTitles;

    private void Awake()
    {
        Challenges = Resources.Load<ChallengesSO>("SO/Challenges").Challenges;

        foreach (ChallengeCategory category in Enum.GetValues(typeof(ChallengeCategory)))
        {
            ChallengeCategories[category] = true;
        }
    }

    public string SelectNewChallenge()
    {
        List<string> challenges = new();

        foreach (var challenge in Challenges)
        {
            if (ChallengeCategories[challenge.Value])
            {
                challenges.Add(challenge.Key);
            }
        }

        int rand = UnityEngine.Random.Range(0, challenges.Count);
        return challenges[rand];
    }

    public void UpdateActiveChallenges(ChallengeCategory category, bool isActive)
    {
        ChallengeCategories[category] = isActive;
    }
}