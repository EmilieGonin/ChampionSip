using TMPro;
using UnityEngine;

public class HUDLobbyButtons : HUDButton
{
    [SerializeField] private SceneHandler _partySceneHandler;
    [SerializeField] private TMP_InputField _inputField;

    public async void CreateLobby()
    {
        Click();
        if (!await GameManager.Instance.Mod<ModLobby>().CreateLobby()) return;
        _partySceneHandler.Load();
    }

    public async void JoinLobby()
    {
        Click();
        if (!await GameManager.Instance.Mod<ModLobby>().JoinLobby(_inputField.text)) return;
    }
}