using UnityEngine;
public class S_WS_View : S_WidgetSwitcher
{
    #region External
    [SerializeField] S_HUD.WS_Type WS_Type;
    #endregion External

    #region MonoBehavior
    protected override void OnEnable()
    {
        base.OnEnable();
        S_HUD.m_WS_EventHandler += WS_Listener;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        S_HUD.m_WS_EventHandler -= WS_Listener;
    }

    private void WS_Listener(S_HUD.WS_Type ws_Type, int _index)
    {
        if (WS_Type == ws_Type)
        {
            SetWidgetActive(_index);
        }
       
    }
    #endregion MonoBehavior
}
