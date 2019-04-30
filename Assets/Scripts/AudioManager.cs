using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public static event Action<bool> SoundStateChanged;

    [SerializeField] private AudioClip _defaultClickClip;

    public static AudioClip DefaultClickClip => Instance._defaultClickClip;

    public static bool IsSoundEnable
    {
        get { return PlayerPrefs.GetInt(nameof(IsSoundEnable), 1) == 1; }
        set
        {
            if (value == IsSoundEnable)
            {
                return;
            }

            PlayerPrefs.SetInt(nameof(IsSoundEnable),value?1:0);
            SoundStateChanged?.Invoke(value);
        }
    }
}