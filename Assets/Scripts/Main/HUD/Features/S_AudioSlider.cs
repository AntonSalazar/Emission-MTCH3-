using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class S_AudioSlider : MonoBehaviour
{
    #region External
    [SerializeField] bool _Sound, _Music;

    bool g_Slider;
    Slider _Slider;
    Slider m_Slider
    {
        get
        {
            if (!g_Slider)
                if (_Slider = GetComponent<Slider>()) g_Slider = true;
            return _Slider;
        }
    }
    #endregion External

    #region MonoBehavior
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (_Sound) m_Slider.value = S_PlayerPreference.m_SoundVolume;
        else if (_Music) m_Slider.value = S_PlayerPreference.m_MusicVolume;
        else if (_Sound && _Music) m_Slider.value = S_PlayerPreference.m_GlobalVolume;
    }
    #region Public
    public void OnValueChange()
    {
        if (_Sound) S_AudioManager.SetVolumeSound(m_Slider.value);
        else if (_Music) S_AudioManager.SetVolumeMusic(m_Slider.value);
        else if (_Sound && _Music) S_AudioManager.SetVolumeGlobal(m_Slider.value);
    }
    #endregion Public

    #endregion MonoBehavior
}
