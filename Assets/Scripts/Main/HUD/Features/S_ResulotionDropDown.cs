using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Dropdown))]
public class S_ResulotionDropDown : MonoBehaviour
{
    #region External
    [SerializeField] Toggle _isFullScreenToggle;
    Resolution[] _Resolutions;

    bool g_Dropdown;
    Dropdown _Dropdown;
    Dropdown m_Dropdown
    {
        get
        {
            if (!g_Dropdown)
                if (_Dropdown = GetComponent<Dropdown>()) g_Dropdown = true;
            return _Dropdown;
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
        _Resolutions = Screen.resolutions;
        Screen.SetResolution(_Resolutions[S_PlayerPreference.m_Resolution].width, _Resolutions[S_PlayerPreference.m_Resolution].height, System.Convert.ToBoolean(S_PlayerPreference.m_Fullscreen));
        _isFullScreenToggle.isOn = System.Convert.ToBoolean(S_PlayerPreference.m_Fullscreen);

        m_Dropdown.ClearOptions();

        List<string> _options = new List<string>();
        for (int i = _Resolutions.Length - 1; i >= 0; i--)
        {
            string _resolution = _Resolutions[i].width + "x" + _Resolutions[i].height;
            _options.Add(_resolution);
        }
        m_Dropdown.AddOptions(_options);
        m_Dropdown.value = _Resolutions.Length - 1 - S_PlayerPreference.m_Resolution;
    }

    #region Public
    public void OnResolutionValueChange(int _index)
    {
        _index = _Resolutions.Length - 1 - _index;
        S_PlayerPreference.m_Resolution = _index;
        Screen.SetResolution(_Resolutions[_index].width, _Resolutions[_index].height, System.Convert.ToBoolean(S_PlayerPreference.m_Fullscreen));
    }

    public void OnFullscreenValueChange(bool _value)
    {
        S_PlayerPreference.m_Fullscreen = System.Convert.ToInt32(_value);
        Screen.SetResolution(_Resolutions[S_PlayerPreference.m_Resolution].width, _Resolutions[S_PlayerPreference.m_Resolution].height, _value);
    }
    #endregion Public

    #endregion MonoBehavior
}
