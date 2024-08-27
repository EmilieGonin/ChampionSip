using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDBottomsUpEffect : MonoBehaviour
{
    [SerializeField, Scene] private string _bottomsUpPopup;

    private void Awake()
    {
        PlayerNetwork.OnBottomsUp += PlayerNetwork_OnBottomsUp;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnBottomsUp -= PlayerNetwork_OnBottomsUp;
    }

    private void PlayerNetwork_OnBottomsUp() => SceneManager.LoadScene(_bottomsUpPopup, LoadSceneMode.Additive);
}