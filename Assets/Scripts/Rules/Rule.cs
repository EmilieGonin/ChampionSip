using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rule : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    [Header("Ressources")]
    [SerializeField] private Sprite[] _sprites;

    private int _sprite = 0;

    public void Init(string text)
    {
        _text.text = text;
    }

    public void Init(string text, int icon)
    {
        _text.text = text;
        _sprite = icon;
        _image.sprite = _sprites[_sprite];
    }

    public void Change()
    {
        if (_sprite == 0) _sprite = 1;
        else _sprite = 0;

        _image.sprite = _sprites[_sprite];

        GameManager.Instance.ChangeRule(_text.text, _sprite);
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