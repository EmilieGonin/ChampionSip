using System;
using UnityEngine;

public class ModGameData : Module
{
    public GameData Data { get; private set; }
    private string _savePath;

    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/GameData.json";

        Data = new();

        ModEconomy.OnCurrencyUpdate += UpdateCurrency;
    }

    private void OnDestroy()
    {
        ModEconomy.OnCurrencyUpdate -= UpdateCurrency;
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(Data);
        System.IO.File.WriteAllText(_savePath, json);
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(_savePath))
        {
            string json = System.IO.File.ReadAllText(_savePath);
            Data = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            Debug.Log("Aucune sauvegarde trouvée.");
            NewGameData();
        }
    }

    public void NewGameData()
    {
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