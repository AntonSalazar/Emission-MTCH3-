using System.Linq;
using UnityEngine;
public class S_HUD : MonoBehaviour
{
    #region External
    internal enum WS_Type
    {
        WS_Panels,
        WS_Menu,
        WS_Settings,
        WS_Pause
    }

    bool _ShowPauseMenu;

    internal delegate void WS_Handler(WS_Type ws_Type, int _index);
    static event WS_Handler _WS_EventHandler = (ws_Type, _index) => { };
    internal static event WS_Handler m_WS_EventHandler
    {
        add
        {
            if (!_WS_EventHandler.GetInvocationList().Contains(value))
            {
                _WS_EventHandler += value;
            }
        }
        remove
        {
            if (_WS_EventHandler.GetInvocationList().Contains(value))
            {
                _WS_EventHandler -= value;
            }
        }
    }
    #endregion External

    #region MonoBehavior

    private void Awake()
    {
        DontDestroyOnLoad(transform.parent);
    }

    private void OnEnable()
    {
        S_PlayerController.m_InputHandler += EscapeButtonDown;
    }

    private void OnDisable()
    {
        S_PlayerController.m_InputHandler -= EscapeButtonDown;
    }

    private void EscapeButtonDown(KeyCode _keyCode, S_PlayerController.KeyState _keyState)
    {
        if (GM_Main.m_GameState != GM_Main.GameState.MainMenu)
        {
            if (_keyCode == KeyCode.Escape)
            {
                if (_keyState == S_PlayerController.KeyState.Down)
                {
                    _ShowPauseMenu = !_ShowPauseMenu;
                    if (_ShowPauseMenu) G_OnBackButton();
                    else G_OnBackToGameButton();
                }
            }
        }
    }

    #region Public

    #region MainMenu
    public void MM_OnPlayButton()
    {
        _WS_EventHandler(WS_Type.WS_Menu, 1);
    }

    public void MM_OnSettingsButton()
    {
        _WS_EventHandler(WS_Type.WS_Menu, 2);
    }

    public void MM_OnBackButton()
    {
        _WS_EventHandler(WS_Type.WS_Menu, 0);
    }

    public void MM_OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion MainMenu

    #region Gameplay
    public void G_OnBackToGameButton()
    {
        _WS_EventHandler(WS_Type.WS_Pause, 0);
        GM_Main.m_GameState = GM_Main.GameState.Gameplay;
        _ShowPauseMenu = false;
        S_AudioManager.SetLowpassFreq(1.0f);
    }

    public void G_OnBackButton()
    {
        _WS_EventHandler(WS_Type.WS_Pause, 1);
        GM_Main.m_GameState = GM_Main.GameState.Pause;
        S_AudioManager.SetLowpassFreq(0.05f);
    }

    public void G_OnSettingsButton()
    {
        _WS_EventHandler(WS_Type.WS_Pause, 2);
    }

    public void G_OnBackIntoMenuButton()
    {
        _WS_EventHandler(WS_Type.WS_Pause, 0);
        _WS_EventHandler(WS_Type.WS_Panels, 0);
        _ShowPauseMenu = false;
        S_AudioManager.SetLowpassFreq(1.0f);
        GM_Main.m_GameState = GM_Main.GameState.MainMenu;
    }
 #endregion Gameplay

    #region SetDificult
    private void StartGame()
    {
        _WS_EventHandler(WS_Type.WS_Menu, 0);
        _WS_EventHandler(WS_Type.WS_Panels, 1);
        GM_Main.m_GameState = GM_Main.GameState.Gameplay;
    }

    public void D_OnEasyButton()
    {
        StartGame();
    }

    public void D_OnMediumButton()
    {
        StartGame();
    }

    public void D_OnHardButton()
    {
        StartGame();
    }
    #endregion SetDificult

    #region Audio
    public void MM_PlayFX()
    {
        S_AudioManager.PlayClick();
    }
    #endregion

    #endregion Public
    #endregion MonoBehavior
}
