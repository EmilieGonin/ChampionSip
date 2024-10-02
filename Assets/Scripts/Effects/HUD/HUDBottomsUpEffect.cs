using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDBottomsUpEffect : MonoBehaviour
{
    [SerializeField, Scene] private string _bottomsUpPopup;

    private void Awake() => EffectSO.OnInflict += EffectSO_OnInflict;
    private void OnDestroy() => EffectSO.OnInflict -= EffectSO_OnInflict;

    private void EffectSO_OnInflict(EffectSO effect, ulong id)
    {
        if (id != GameManager.Instance.PlayerId) return;
        if (effect is not BottomsUp) return;
        SceneManager.LoadScene(_bottomsUpPopup, LoadSceneMode.Additive);
    }
}