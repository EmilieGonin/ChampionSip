using TMPro;
using UnityEngine;

public class LobbyButtons : MonoBehaviour
{
    [SerializeField] private SceneHandler _sceneHandler;
    [SerializeField] private TMP_InputField _inputField;

    private void Awake()
    {
        GameManager.OnLobbyCreated += _sceneHandler.Load;
    }

    private void OnDestroy()
    {
        GameManager.OnLobbyCreated -= _sceneHandler.Load;
    }

    public void CreateLobby() => GameManager.Instance.Mod<ModLobby>().CreateLobby();
    public void JoinLobby() => GameManager.Instance.Mod<ModLobby>().JoinLobby(_inputField.text);
}