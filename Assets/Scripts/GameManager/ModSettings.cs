using System;
using System.Collections.Generic;

public enum Setting
{
    Sounds,
    Vibration
}

public class ModSettings : Module
{
    public Dictionary<Setting, bool> Settings { get; private set; } = new();

    private void Awake()
    {
        HUDSetting.OnSettingToggle += HUDSetting_OnSettingToggle;

        foreach (Setting setting in Enum.GetValues(typeof(Setting)))
        {
            Settings[setting] = true;
        }
    }

    private void OnDestroy()
    {
        HUDSetting.OnSettingToggle -= HUDSetting_OnSettingToggle;
    }

    private void HUDSetting_OnSettingToggle(Setting setting, bool isActive)
    {
        Settings[setting] = isActive;
    }
}