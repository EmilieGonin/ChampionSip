using TMPro;
using UnityEngine;

public class Rule : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void Init(string text)
    {
        _text.text = text;
    }

    public void Add()
    {
        GameManager.Instance.AddRule(_text.text);
    }

    public void Remove()
    {
        GameManager.Instance.RemoveRule(_text.text);
        Destroy(gameObject);
    }
}