using UnityEngine;

[CreateAssetMenu(fileName = "SipTransfer", menuName = "Game/Effects/SipTransfer")]
public class SipTransfer : EffectSO
{
    public override void Activate()
    {
        if (GameManager.Instance.SipsToDrink < 5)
        {
            GameManager.Instance.ShowError("Pas assez de gorg�es � transf�rer.");
            return;
        }

        base.Activate();
    }
}