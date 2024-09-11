using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnLobbyCreated;

    public static event Action OnRulesChange;

    [SerializeField] private List<Module> _modules = new();

    [Header("Scenes")]
    public SceneHandler LoadingScreenSceneHandler;
    public SceneHandler ErrorSceneHandler;
    public SceneHandler NotificationSceneHandler;

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

        Challenges = Resources.Load<ChallengesSO>("SO/Challenges").Challenges;

        Effects = Resources.LoadAll<EffectSO>("SO/Effects").OrderBy(x => x.Price).ToList();

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
        EffectSO.OnInflict += EffectSO_OnInflict;
        Counter.OnNewSip -= Counter_OnNewSip;
        PlayerNetwork.OnChallengeCompleted -= PlayerNetwork_OnChallengeCompleted;
    }

    public T Mod<T>() where T : Module => _modules.OfType<T>().First();

    public void InvokeOnLobbyCreated() => OnLobbyCreated?.Invoke();

    private void Counter_OnNewSip(int amount) => Sips += amount;

    #region Economy
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

    #region Challenges
    private void PlayerNetwork_OnChallengeCompleted(bool victory)
    {
        if (victory) AddGold(10);
    }
    #endregion

    public EffectSO GetEffectByName(string name) => Effects.Find(x => x.Name == name);

    private void EffectSO_OnActivate(EffectSO effect)
    {
        CurrentEffects.Add(effect);
        RemoveGold(effect.Price);
        ShowNotification($"Vous avez activé l'effet <b>{effect.Name}</b> !");
    }

    private void EffectSO_OnDeactivate(EffectSO effect) => CurrentEffects.Remove(effect);
    private void EffectSO_OnInflict(EffectSO effect) => ShowNotification($"On vous a infligé l'effet <b>{effect.Name}</b> !");

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