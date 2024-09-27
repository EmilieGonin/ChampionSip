using UnityEngine;
using UnityEngine.UI;

public class HUDToggle : MonoBehaviour
{
    [SerializeField] private Image _toggle;
    [SerializeField] private Sprite _activeToggleSprite;
    [SerializeField] private Sprite _unactiveToggleSprite;

    protected bool _isActive;

    public void Init(bool isActive)
    {
        _isActive = isActive;
        SetSprite();
    }

    public virtual void Toggle()
    {
        _isActive = !_isActive;
        SetSprite();
    }

    private void SetSprite()
    {
        Sprite toggleSprite = _isActive ? _activeToggleSprite : _unactiveToggleSprite;
        _toggle.sprite = toggleSprite;
    }
}