using UnityEngine;

[CreateAssetMenu(fileName = "SipTransfer", menuName = "Game/Effects/SipTransfer")]
public class SipTransfer : EffectSO
{
    public override void Activate()
    {
        if (GameManager.Instance.Currencies[Currency.SipsToDrink] < 5)
        {
            GameManager.Instance.ShowError("Pas assez de gorgées à transférer.");
            return;
        }

        base.Activate();
    }
}