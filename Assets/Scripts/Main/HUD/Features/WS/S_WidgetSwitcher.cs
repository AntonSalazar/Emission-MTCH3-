using UnityEngine;
public class S_WidgetSwitcher : MonoBehaviour
{
    #region External
    [Header("Settings")]
    [SerializeField] GameObject[] _Widgets;
    static bool _Inited;

    int _PrevIndex = 0;
    int _NextIndex;
    #endregion External

    #region MonoBehavior
    protected virtual void Awake()
    {
        Init();
    }
    protected virtual void OnEnable()
    {
        
    }
    protected virtual void OnDisable()
    {

    }
    private void Init()
    {
        for (int i = 0; i < _Widgets.Length; i++)
        {
            if (_Widgets[i].activeSelf)
            {
                _NextIndex = _PrevIndex = i;
                break;
            }
        }
    }

    #region WidgetActive
    internal void SetWidgetActive(int _index)
    {
        if (_PrevIndex != -1) _Widgets[_PrevIndex].SetActive(false);
        if (_index >= _Widgets.Length) _index = 0;
        else if (_index < 0) _index = _Widgets.Length - 1;
        _Widgets[_index].SetActive(true);
        _PrevIndex = _index;
        PlayerPrefs.SetInt("WS_Prev" + gameObject.name, _index);
        
    }

    #region Public
    public void OnNextWidget()
    {
        _NextIndex = (_NextIndex < _Widgets.Length - 1) ? ++_NextIndex : 0;
        PlayerPrefs.SetInt("WS_Next" + gameObject.name, _NextIndex);
        SetWidgetActive(_NextIndex);
    }

    public void OnPrevWidget()
    {
        _NextIndex = (_NextIndex > 0) ? --_NextIndex : _Widgets.Length - 1;
        PlayerPrefs.SetInt("WS_Next" + gameObject.name, _NextIndex);
        SetWidgetActive(_NextIndex);
    }
    #endregion Public

    #endregion WidgetActive

    #endregion MonoBehavior
}
