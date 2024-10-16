using System.Collections.Generic;
using UnityEngine;

public class ModGameData : Module
{
    public GameData Data { get; private set; }
    public bool IsNewGame { get; private set; }
    private string _savePath;

    public Dictionary<Currency, int> CurrenciesData;

    private void Awake()
    {
        ModEconomy.OnCurrencyUpdate += UpdateCurrency;

        _savePath = Application.persistentDataPath + "/GameData.json";

        LoadGame();
    }

    private void OnDestroy()
    {
        ModEconomy.OnCurrencyUpdate -= UpdateCurrency;
    }

    private void SaveGame()
    {
        string json = JsonUtility.ToJson(Data);
        System.IO.File.WriteAllText(_savePath, json);
    }

    private void LoadGame()
    {
        if (System.IO.File.Exists(_savePath))
        {
            string json = System.IO.File.ReadAllText(_savePath);
            Data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Save file found at " + _savePath);
            IsNewGame = false;
        }
        else
        {
            Debug.Log("Aucune sauvegarde trouv�e.");
            NewGameData();
        }
    }

    public void NewGameData()
    {
        IsNewGame = true;
        Data = new();
        SaveGame();
    }

    private void UpdateCurrency(Currency currency, int amount)
    {
        switch (currency)
        {
            case Currency.Golds:
                Data.Golds = amount;
                break;
            case Currency.SipsToDrink:
                Data.SipsToDrink = amount;
                break;
            case Currency.Sips:
                Data.Sips = amount;
                break;
            case Currency.Shots:
                Data.Shots = amount;
                break;
        }

        SaveGame();
    }
}