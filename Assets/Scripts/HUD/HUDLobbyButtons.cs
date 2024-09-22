using TMPro;
using UnityEngine;

public class HUDLobbyButtons : MonoBehaviour
{
    [SerializeField] private SceneHandler _sceneHandler;
    [SerializeField] private TMP_InputField _inputField;

    //private void Awake()
    //{
    //    GameManager.OnLobbyCreated += _sceneHandler.Load;
    //}

    //private void OnDestroy()
    //{
    //    GameManager.OnLobbyCreated -= _sceneHandler.Load;
    //}

    public async void CreateLobby()
    {
        if (!await GameManager.Instance.Mod<ModLobby>().CreateLobby()) return;
        _sceneHandler.Load();
    }

    public async void JoinLobby()
    {
        if (!await GameManager.Instance.Mod<ModLobby>().JoinLobby(_inputField.text)) return;
        _sceneHandler.Load();
    }
}