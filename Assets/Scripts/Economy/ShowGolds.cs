using TMPro;
using UnityEngine;

public class ShowGolds : MonoBehaviour
{
    [SerializeField] private TMP_Text _gold;

    private int _goldInt;

    private void Awake()
    {
        Economy.OnAddGold += Economy_OnAddGold;
        EffectSO.OnBuy += EffectSO_OnBuy;
    }

    private void Start()
    {
        _goldInt = GameManager.Instance.Gold;
        _gold.text = _goldInt.ToString();
    }

    private void OnDestroy()
    {
        Economy.OnAddGold -= Economy_OnAddGold;
        EffectSO.OnBuy -= EffectSO_OnBuy;
    }

    private void Economy_OnAddGold(int amount)
    {
        _goldInt += amount;
        _gold.text = _goldInt.ToString();
    }

    private void EffectSO_OnBuy(int amount)
    {
        _goldInt -= amount;
        _gold.text = _goldInt.ToString();
    }
}