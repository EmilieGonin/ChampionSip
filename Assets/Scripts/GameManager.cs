using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnRulesChange;

    public Dictionary<string, int> Rules { get; private set; } = new();
    public Dictionary<string, int> RulesPrefabs { get; private set; } = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        RulesSO rulesSO = Resources.Load<RulesSO>("SO/Rules");

        foreach (var rule in rulesSO.Rules)
        {
            RulesPrefabs.Add(rule.Key, 0);
        }
    }

    public void AddRule(string rule)
    {
        Rules.Add(rule, 0);
        RulesPrefabs.Remove(rule);
        OnRulesChange?.Invoke();
    }

    public void ChangeRule(string rule, int icon)
    {
        Rules[rule] = icon;
    }

    public void RemoveRule(string rule)
    {
        RulesPrefabs.Add(rule, 0);
        Rules.Remove(rule);
        OnRulesChange?.Invoke();
    }
}