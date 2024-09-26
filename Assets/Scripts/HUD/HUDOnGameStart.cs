using UnityEngine;

public class HUDOnGameStart : MonoBehaviour
{
    [SerializeField] private SceneHandler _sceneHandler;

    private void Start()
    {
        if (GameManager.Instance.IsNewGame) GameManager.Instance.InitModules();
        else _sceneHandler.Load();
    }
}