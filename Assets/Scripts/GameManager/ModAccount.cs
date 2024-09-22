using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class ModAccount : Module
{
    public string PlayerName { get; private set; }
    private const string _namesPath = "Assets/Resources/PlayerNames.txt";

    async private void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
            await SignInAnonymouslyAsync();
            ChooseRandomName();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            _manager.ShowError(err.Message);
        };

        AuthenticationService.Instance.SignedOut += () => {
            _manager.ShowError("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            _manager.ShowError("Player session could not be refreshed and expired.");
        };
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private void ChooseRandomName()
    {
        List<string> names = new();

        if (File.Exists(_namesPath))
        {
            string[] lines = File.ReadAllLines(_namesPath);
            names.AddRange(lines);
        }

        int randomIndex = UnityEngine.Random.Range(0, names.Count);
        PlayerName = names[randomIndex];

        Debug.Log("Player name is " + PlayerName);
    }
}