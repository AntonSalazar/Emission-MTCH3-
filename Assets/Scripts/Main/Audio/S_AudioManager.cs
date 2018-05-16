using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class S_AudioManager : S_IniterObject
{
    #region External
    [Header("Outputs")]
    [SerializeField] AudioSource _SoundOutput;
    [SerializeField] AudioSource _MusicOutput;

    [Header("SFX")]
    [SerializeField] AudioMixer _AM_AudioManager;
    [SerializeField] AudioClip _ClickClip;
    [SerializeField] AudioClip[] _Musics;

    static float _Time, _TimeStay;
    #endregion External
    #region Instance
    static S_AudioManager _Instance;
    internal static S_AudioManager m_Instance
    {
        private set { _Instance = value; }
        get { return _Instance; }
    }
    #endregion Instance

    #region Play
    internal static void PlayClick(float _volume = 1.0f)
    {
        PlayOneShot(m_Instance._ClickClip, _volume);
    }

    internal static void PlayOneShot(AudioClip _clip, float _volume = 1.0f)
    {
        m_Instance._SoundOutput.PlayOneShot(_clip, _volume);
    }

    internal static void PlayClip(AudioClip _clip)
    {
        if(_TimeStay <= 0.0f) m_Instance._MusicOutput.clip = _clip;
        m_Instance._MusicOutput.time = _TimeStay;
        m_Instance._MusicOutput.Play();
    }

    internal static void SetLowpassFreq(float _value)
    {
        m_Instance._AM_AudioManager.SetFloat("LowpassFreq", Mathf.Lerp(0.0f, 22000.0f, _value));
    }

    private AudioClip GetRandomClip()
    {
        return _Musics[Random.Range(0, _Musics.Length)];
    }

    private IEnumerator PlayRandomMusic()
    {
        
        if(_TimeStay > 0.0f) yield return new WaitForSeconds(0.01f);

        PlayClip(GetRandomClip());
        yield return new WaitForSeconds(m_Instance._MusicOutput.clip.length - _TimeStay + 0.1f);

        _TimeStay = 0.0f;
        StartCoroutine(PlayRandomMusic());
    }
    #endregion

    #region SetVolume
    internal static void SetVolumeGlobal(float _value)
    {
        S_PlayerPreference.m_GlobalVolume = _value;
        m_Instance._AM_AudioManager.SetFloat("GlobalVolume", Mathf.Lerp(-80.0f, 0.0f, _value));
    }

    internal static void SetVolumeMusic(float _value)
    {
        S_PlayerPreference.m_MusicVolume = _value;
        m_Instance._MusicOutput.volume = _value;
    }

    internal static void SetVolumeSound(float _value)
    {
        S_PlayerPreference.m_SoundVolume = _value;
        m_Instance._SoundOutput.volume = _value;
    }
    #endregion SetVolume

    #region MonoBehavior
    protected override void Awake()
    {
        base.Awake();

        m_Instance = this;
        DontDestroyOnLoad(transform);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GM_Main.m_TickHandler += Tick;
        StartCoroutine(PlayRandomMusic());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        GM_Main.m_TickHandler -= Tick;
        _TimeStay = _Time;
    }

    private void Start()
    {
        SetVolumeGlobal(S_PlayerPreference.m_GlobalVolume);
        SetVolumeMusic(S_PlayerPreference.m_MusicVolume);
        SetVolumeSound(S_PlayerPreference.m_SoundVolume);
    }

    private void Tick(GM_Main.GameState _gameState)
    {
        _Time = _MusicOutput.time;
    }
    #endregion MonoBehavior
}
