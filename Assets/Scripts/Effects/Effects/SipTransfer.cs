using UnityEngine;

[CreateAssetMenu(fileName = "SipTransfer", menuName = "Game/Effects/SipTransfer")]
public class SipTransfer : EffectSO
{
    public override bool CanBuy()
    {
        if (GameManager.Instance.Currencies[Currency.SipsToDrink] < 5) return false;
        return base.CanBuy();
    }
}