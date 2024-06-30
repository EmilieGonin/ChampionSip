using System.Collections.Generic;
using UnityEngine;

public class LoadRules : MonoBehaviour
{
    [SerializeField] private GameObject _rulePrefab;
    [SerializeField] private bool _rulesPrefabs;

    private List<string> _rules;

    private void Awake()
    {
        if (_rulesPrefabs) _rules = GameManager.Instance.RulesPrefabs;
        else _rules = GameManager.Instance.Rules;
        Load();

        GameManager.OnRulesChange += GameManager_OnRulesChange;
    }

    private void OnDestroy()
    {
        GameManager.OnRulesChange -= GameManager_OnRulesChange;
    }

    private void GameManager_OnRulesChange()
    {
        if (_rulesPrefabs) return;
        _rules = GameManager.Instance.Rules;
        Load();
    }

    private void Load()
    {
        foreach (string rule in _rules)
        {
            Rule ruleObject = Instantiate(_rulePrefab, transform).GetComponent<Rule>();
            ruleObject.Init(rule);
        }
    }
}