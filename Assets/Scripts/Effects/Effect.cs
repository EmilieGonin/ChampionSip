using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    [SerializeField] private TMP_Text _effectName;
    [SerializeField] private TMP_Text _effectDescription;
    [SerializeField] private TMP_Text _effectPrice;
    [SerializeField] private Image _effectIcon;

    private EffectSO _effect;

    public void Init(EffectSO effect)
    {
        _effect = effect;
        _effectName.text = effect.Name;
        _effectDescription.text = effect.Description;
        _effectPrice.text = effect.Price.ToString();
    }

    public void Activate()
    {
        //deduct price
        _effect.Activate();
    }
}