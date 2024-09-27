using System;
using UnityEngine;

public class HUDSetting : HUDToggle
{
    public static event Action<Setting, bool> OnSettingToggle;

    [SerializeField] private Setting _setting;

    private void Awake()
    {
        Init(GameManager.Instance.Settings[_setting]);
    }

    public override void Toggle()
    {
        base.Toggle();
        OnSettingToggle?.Invoke(_setting, _isActive);
    }
}