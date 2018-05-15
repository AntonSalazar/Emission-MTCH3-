using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class S_QualityDropdown : MonoBehaviour
{
    #region External
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
        m_Dropdown.value = S_PlayerPreference.m_Quality;
        m_Dropdown.RefreshShownValue();
    }

    #region Public
    public void OnValueChange(int _index)
    {
        if (QualitySettings.GetQualityLevel() != _index)
        {
            S_PlayerPreference.m_Quality = _index;
            QualitySettings.SetQualityLevel(_index);
        }
    }
    #endregion Public
    #endregion MonoBehavior
}
