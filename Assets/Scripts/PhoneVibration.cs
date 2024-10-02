using UnityEngine;

public class PhoneVibration : MonoBehaviour
{
    private void Awake()
    {
        EffectSO.OnActivate += EffectSO_OnActivate;
        EffectSO.OnInflict += EffectSO_OnInflict;
        PlayerNetwork.OnChallengeSelect += PlayerNetwork_OnChallengeSelect;
    }

    private void OnDestroy()
    {
        EffectSO.OnActivate -= EffectSO_OnActivate;
        EffectSO.OnInflict -= EffectSO_OnInflict;
        PlayerNetwork.OnChallengeSelect -= PlayerNetwork_OnChallengeSelect;
    }

    private void EffectSO_OnActivate(EffectSO effect, ulong id) => VibrateShort();
    private void EffectSO_OnInflict(EffectSO effect, ulong id) => VibrateMedium();
    private void PlayerNetwork_OnChallengeSelect(string challenge) => VibrateMedium();

    public void VibrateShort()
    {
        //
    }
    public void VibrateMedium() => Vibrate();
    public void VibrateLong() => Vibrate();

    private void Vibrate()
    {
        if (!GameManager.Instance.Settings[Setting.Vibration]) return;
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }

    private void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

        if (vibrator.Call<bool>("hasVibrator"))
        {
            vibrator.Call("vibrate", milliseconds);
        }
#endif
    }
}