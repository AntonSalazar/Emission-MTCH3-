using UnityEngine;

public class S_PlayerPreference : MonoBehaviour
{
    #region External
    #region Audio
    static float _SoundVolume = -1.0f;
    internal static float m_SoundVolume
    {
        set
        {
            if (_SoundVolume != value)
            {
                _SoundVolume = value;
                PlayerPrefs.SetFloat("Sound", _SoundVolume);
                PlayerPrefs.Save();
            }
        }
        get
        {
            if (_SoundVolume < 0.0f) _SoundVolume = PlayerPrefs.GetFloat("Sound", 1.0f);
            return _SoundVolume;
        }
    }

    static float _MusicVolume = -1.0f;
    internal static float m_MusicVolume
    {
        set
        {
            if (_MusicVolume != value)
            {
                _MusicVolume = value;
                PlayerPrefs.SetFloat("Music", _MusicVolume);
                PlayerPrefs.Save();
            }
        }
        get
        {
            if (_MusicVolume < 0.0f) _MusicVolume = PlayerPrefs.GetFloat("Music", 1.0f);
            return _MusicVolume;
        }
    }

    static float _GlobalVolume = -1.0f;
    internal static float m_GlobalVolume
    {
        set
        {
            if (_GlobalVolume != value)
            {
                _GlobalVolume = value;
                PlayerPrefs.SetFloat("GlobalVolume", _GlobalVolume);
                PlayerPrefs.Save();
            }
        }
        get
        {
            if (_GlobalVolume < 0.0f) _GlobalVolume = PlayerPrefs.GetFloat("GlobalVolume", 1.0f);
            return _GlobalVolume;
        }
    }
    #endregion

    #region Graphics
    static int _Quality = -1;
    internal static int m_Quality
    {
        set
        {
            if (_Quality != value)
            {
                _Quality = value;
                PlayerPrefs.SetInt("Quality", _Quality);
                PlayerPrefs.Save();
            }
        }
        get
        {
            if (_Quality < 0) _Quality = PlayerPrefs.GetInt("Quality", 5);
            return _Quality;
        }
    }

    static int _Resolution = -1;
    internal static int m_Resolution
    {
        set
        {
            if (_Resolution != value)
            {
                _Resolution = value;
                PlayerPrefs.SetInt("Resolution", _Resolution);
                PlayerPrefs.Save();
            }
        }
        get
        {
            if (_Resolution < 0) _Resolution = PlayerPrefs.GetInt("Resolution", Screen.resolutions.Length - 1);
            return _Resolution;
        }
    }

    static int _Fullscreen = -1;
    internal static int m_Fullscreen
    {
        set
        {
            if (_Fullscreen != value)
            {
                _Fullscreen = value;
                PlayerPrefs.SetInt("Fullscreen", _Fullscreen);
                PlayerPrefs.Save();
            }
        }
        get
        {
            if (_Fullscreen < 0) _Fullscreen = PlayerPrefs.GetInt("Fullscreen", 1);
            return _Fullscreen;
        }
    }
    #endregion
    #endregion External

    #region MonoBehavior
    internal static void Clear()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    #endregion MonoBehavior
}
