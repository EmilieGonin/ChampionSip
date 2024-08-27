using TMPro;
using UnityEngine;

public class HUDChallenge : MonoBehaviour
{
    [SerializeField] private TMP_Text _challenge;

    private void Awake()
    {
        PlayerNetwork.OnChallengeSelect += SelectChallenge_OnChallengeSelect;
    }

    private void OnDestroy()
    {
        PlayerNetwork.OnChallengeSelect -= SelectChallenge_OnChallengeSelect;
    }

    private void SelectChallenge_OnChallengeSelect()
    {
        _challenge.text = $"New challenge";
    }
}