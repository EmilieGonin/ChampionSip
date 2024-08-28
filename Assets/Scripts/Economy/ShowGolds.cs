using TMPro;
using UnityEngine;

public class ShowGolds : MonoBehaviour
{
    [SerializeField] private TMP_Text _gold;

    private int _goldInt;

    private void Update()
    {
        _goldInt = GameManager.Instance.Gold;
        _gold.text = _goldInt.ToString();
    }
}