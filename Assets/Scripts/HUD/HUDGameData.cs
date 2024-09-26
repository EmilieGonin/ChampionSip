using UnityEngine;

public class HUDGameData : HUDButton
{
    [SerializeField] private SceneHandler _sceneHandler;

    public void LoadGame(bool reset)
    {
        Click();
        if (reset) GameManager.Instance.NewGameData();
        GameManager.Instance.InitModules();
        _sceneHandler.Unload();
    }
}