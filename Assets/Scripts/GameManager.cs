using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnRulesChange;

    public List<string> Rules { get; private set; } = new();
    public List<string> RulesPrefabs { get; private set; } = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        RulesPrefabs.Add("Test");
    }

    public void AddRule(string rule)
    {
        Rules.Add(rule);
        RulesPrefabs.Remove(rule);
        OnRulesChange?.Invoke();
    }

    public void RemoveRule(string rule)
    {
        RulesPrefabs.Add(rule);
        Rules.Remove(rule);
        OnRulesChange?.Invoke();
    }
}