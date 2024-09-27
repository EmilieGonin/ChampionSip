using TMPro;
using UnityEngine;

public class ChallengeCategorySetting : HUDToggle
{
    [SerializeField] private TMP_Text _name;
    private ChallengeCategory _category;

    public void Init(ChallengeCategory category, bool active)
    {
        _category = category;
        _name.text = GameManager.Instance.ChallengeCategoriesTitles[category];
        Init(active);
    }

    public override void Toggle()
    {
        base.Toggle();
        GameManager.Instance.UpdateActiveChallenges(_category, _isActive);
    }
}