using UnityEngine;
public class S_HUDCamera : S_IniterObject
{
    #region External
    static S_HUDCamera _Instance;

    static bool g_MainCamera;
    static Transform _MainCamera;
    static Transform m_MainCamera
    {
        get
        {
            if (!g_MainCamera)
                if (_MainCamera = GameObject.FindWithTag("MainCamera").transform) g_MainCamera = true;
            return _MainCamera;
        }
    }
    #endregion External

    #region MonoBehavior
    protected override void Awake()
    {
        base.Awake();
        _Instance = this;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GM_Main.m_TickHandler += SetLocation;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        g_MainCamera = false;
        GM_Main.m_TickHandler -= SetLocation;
    }

    internal static void SetLocation(GM_Main.GameState _gameState)
    {
        _Instance.m_Transform.position = m_MainCamera.position;
        _Instance.m_Transform.rotation = m_MainCamera.rotation;
    }
    #endregion MonoBehavior
}
