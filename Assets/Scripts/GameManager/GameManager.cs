using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnRulesChange;

    [SerializeField] private List<Module> _modules = new();

    [Header("Scenes")]
    public SceneHandler LoadingScreenSceneHandler;
    public SceneHandler ErrorSceneHandler;
    public SceneHandler NotificationSceneHandler;

    public GameData Data => Mod<ModGameData>().Data;
    public bool IsNewGame => Mod<ModGameData>().IsNewGame;
    public void NewGameData() => Mod<ModGameData>().NewGameData();

    // Rules
    public Dictionary<string, int> Rules { get; private set; } = new();
    public Dictionary<string, int> RulesPrefabs { get; private set; } = new();
    public Dictionary<Category, Sprite> RulesCategories { get; private set; } = new();

    // Challenges
    public Dictionary<string, ChallengeCategory> Challenges => Mod<ModChallenges>().Challenges;
    public Dictionary<ChallengeCategory, bool> ChallengeCategories => Mod<ModChallenges>().ChallengeCategories;
    public SerializedDictionary<ChallengeCategory, string> ChallengeCategoriesTitles => Mod<ModChallenges>().ChallengeCategoriesTitles;
    public string SelectNewChallenge() => Mod<ModChallenges>().SelectNewChallenge();
    public void UpdateActiveChallenges(ChallengeCategory category, bool isActive) => Mod<ModChallenges>().UpdateActiveChallenges(category, isActive);

    // Account
    public string PlayerName => Mod<ModAccount>().PlayerName;

    // Lobby
    public bool IsHost => Mod<ModLobby>().IsHost;

    // Economy
    public Dictionary<Currency, int> Currencies => Mod<ModEconomy>().Currencies;
    public void AddCurrency(Currency currency, int amount) => Mod<ModEconomy>().AddCurrency(currency, amount);
    public void RemoveCurrency(Currency currency, int amount) => Mod<ModEconomy>().RemoveCurrency(currency, amount);

    //Effects
    public List<EffectSO> Effects => Mod<ModEffects>().Effects;
    public List<EffectSO> CurrentEffects => Mod<ModEffects>().CurrentEffects;

    public EffectSO GetEffectByName(string name) => Mod<ModEffects>().GetEffectByName(name);
    public bool HasEffect<T>() where T : EffectSO => Mod<ModEffects>().HasEffect<T>();

    public string ErrorMessage { get; private set; }
    public string NotificationMessage { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.runInBackground = true;

        RulesSO rulesSO = Resources.Load<RulesSO>("SO/Rules");
        RulesCategories = rulesSO.RulesCategories;

        foreach (var rule in rulesSO.Rules)
        {
            RulesPrefabs.Add(rule.Key, 0);
        }
    }

    public T Mod<T>() where T : Module => _modules.OfType<T>().First();

    public void InitModules()
    {
        foreach (var module in _modules) module.Init();
    }

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

    public void ShowError(string message)
    {
        ErrorMessage = message;
        Debug.LogError(message);
        ErrorSceneHandler.Load();
    }

    public void ShowNotification(string message)
    {
        NotificationMessage = message;
        Debug.Log(message);
        NotificationSceneHandler.Load();
    }
}