using UnityEngine;

public class HUDGameData : MonoBehaviour
{
    [SerializeField] private SceneHandler _sceneHandler;

    public void LoadGame(bool reset)
    {
        if (reset) GameManager.Instance.NewGameData();
        else GameManager.Instance.InitModules();
        _sceneHandler.Load();
    }
}