using UnityEngine;

public class HUDLoadChallengeSettings : MonoBehaviour
{
    [SerializeField] private GameObject _challengeSettingPrefab;

    private void Awake()
    {
        foreach (var category in GameManager.Instance.ChallengeCategories)
        {
            GameObject go = Instantiate(_challengeSettingPrefab, transform);
            go.GetComponent<ChallengeCategorySetting>().Init(category.Key, category.Value);
        }
    }
}