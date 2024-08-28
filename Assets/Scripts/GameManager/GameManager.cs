using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnLobbyCreated;

    public static event Action OnRulesChange;

    [Header("Modules")]
    [SerializeField] private List<Module> _modules = new();

    //Economy
    public int Gold { get; private set; } = 0;

    // Rules
    public Dictionary<string, int> Rules { get; private set; } = new();
    public Dictionary<string, int> RulesPrefabs { get; private set; } = new();
    public Dictionary<Category, Sprite> RulesCategories { get; private set; } = new();

    // Challenges
    public List<string> Challenges { get; private set; } = new();

    //Effects
    public List<EffectSO> Effects { get; private set; } = new();
    public List<EffectSO> CurrentEffects { get; private set; } = new();
    public int Sips { get; private set; } = 0;

    public SceneHandler SceneHandler;

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
        RulesCategories = rulesSO.RulesCategories;

        foreach (var rule in rulesSO.Rules)
        {
            RulesPrefabs.Add(rule.Key, 0);
        }

        Challenges = Resources.Load<ChallengesSO>("SO/Challenges").Challenges;

        Effects = Resources.LoadAll<EffectSO>("SO/Effects").ToList();
        Effects.OrderBy(x => x.Name);

        Economy.OnAddGold += AddGold;
        EffectSO.OnActivate += EffectSO_OnActivate;
        EffectSO.OnDeactivate += EffectSO_OnDeactivate;
        Counter.OnNewSip += Counter_OnNewSip;
        PlayerNetwork.OnChallengeCompleted += PlayerNetwork_OnChallengeCompleted;
    }

    private void OnDestroy()
    {
        Economy.OnAddGold -= AddGold;
        EffectSO.OnActivate -= EffectSO_OnActivate;
        EffectSO.OnDeactivate -= EffectSO_OnDeactivate;
        Counter.OnNewSip -= Counter_OnNewSip;
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
    }

    public T Mod<T>() where T : Module => _modules.OfType<T>().First();

    public void InvokeOnLobbyCreated() => OnLobbyCreated?.Invoke();

    private void Counter_OnNewSip(int amount) => Sips += amount;

    #region Economy
    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        if (victory) AddGold(10);
    }

    private void AddGold(int gold)
    {
        Gold += gold;
    }

    private void RemoveGold(int gold)
    {
        Gold -= gold;
    }
    #endregion

    #region Rules
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
    #endregion

    private void EffectSO_OnActivate(EffectSO effect)
    {
        CurrentEffects.Add(effect);
        RemoveGold(effect.Price);
    }

    private void EffectSO_OnDeactivate(EffectSO effect) => CurrentEffects.Remove(effect);

    public EffectSO GetEffectByName(string name) => Effects.Find(x => x.Name == name);

    public void ShowError(string message)
    {
        Debug.LogError(message);
    }

    public void ShowNotification(string message)
    {
        Debug.Log(message);
    }
}