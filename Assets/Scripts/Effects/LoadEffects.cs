using UnityEngine;
using UnityEngine.UI;

public class LoadEffects : MonoBehaviour
{
    [SerializeField] private GameObject _effectPrefab;

    private void Awake()
    {
        foreach (var effect in GameManager.Instance.Effects)
        {
            Effect effectObject = Instantiate(_effectPrefab, transform).GetComponent<Effect>();
            effectObject.Init(effect);

            if (!effect.CanBuy()) effectObject.GetComponent<Button>().interactable = false;
        }
    }
}