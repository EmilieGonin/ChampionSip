using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum SoundType
{
    NewChallenge,
    ChallengeWon,
    ChallengeLost,
    Sip,
    Click
}

public class ModSounds : Module
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private SerializedDictionary<SoundType, AudioClip> _sounds;

    private void Awake()
    {
        HUDButton.OnClick += PlayButtonSound;
        PlayerNetwork.OnChallengeSelect += PlayNewChallengeSound;
        PlayerNetwork.OnChallengeCompleted += PlayChallengeSound;
        Counter.OnCounterAdd += PlaySipButton;
        Counter.OnCounterRemove += PlaySipButton;
    }

    private void OnDestroy()
    {
        HUDButton.OnClick -= PlayButtonSound;
        PlayerNetwork.OnChallengeSelect -= PlayNewChallengeSound;
        PlayerNetwork.OnChallengeCompleted -= PlayChallengeSound;
        Counter.OnCounterAdd -= PlaySipButton;
        Counter.OnCounterRemove -= PlaySipButton;
    }

    public void PlaySound(SoundType type) => _source.PlayOneShot(_sounds[type]);

    private void PlayButtonSound() => PlaySound(SoundType.Click);
    private void PlayChallengeSound(bool win) => PlaySound(win ? SoundType.ChallengeWon : SoundType.ChallengeLost);
    private void PlayNewChallengeSound(string challenge) => PlaySound(SoundType.NewChallenge);
    private void PlaySipButton(Currency currency, int amount) => PlaySound(SoundType.Sip);
}