using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [HorizontalLine(color: EColor.Green)]
    [BoxGroup("Load Settings"), SerializeField, Scene] private string _sceneToLoad;
    [BoxGroup("Load Settings"), SerializeField] private bool _loadAsAdditive;
    [BoxGroup("Load Settings"), SerializeField] private bool _instantLoad;

    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("Unload Settings"), SerializeField] private bool _unloadCurrentScene = true;
    [BoxGroup("Unload Settings"), HideIf("_unloadCurrentScene"), SerializeField, Scene]
    private string _sceneToUnload;
    [BoxGroup("Unload Settings"), SerializeField] private bool _popupTimeout = false;
    [BoxGroup("Unload Settings"), ShowIf("_popupTimeout"), SerializeField]
    private int _timeoutDuration = 3;

    private LoadSceneMode _loadSceneMode;

    private void Awake()
    {
        _loadSceneMode = _loadAsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
        if (!_instantLoad) return;
        Load();
    }

    public void Load()
    {
        SceneManager.LoadScene(_sceneToLoad, _loadSceneMode);
        if (_popupTimeout) StartCoroutine(Timeout());
    }

    public void Unload()
    {
        string scene = _unloadCurrentScene ? gameObject.scene.name : _sceneToUnload;
        SceneManager.UnloadSceneAsync(scene);
    }

    private IEnumerator Timeout()
    {
        yield return new WaitForSeconds(_timeoutDuration);
        Unload();
        yield return null;
    }
}