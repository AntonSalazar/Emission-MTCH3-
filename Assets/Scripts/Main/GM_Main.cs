using System.Linq;
using UnityEngine;

public class GM_Main : S_IniterObject
{
    #region Enums
    #region GameState
    internal enum GameState
    {
        MainMenu,
        Gameplay,
        Pause
    }
    static GameState _GameState = GameState.MainMenu;
    internal static GameState m_GameState
    {
        set
        {
            if (_GameState != value)
            {
                _GameState = value;
                _GameHandler(m_Dificult, _GameState);
                Debug.Log("Game State set to: " + _GameState.ToString());
            }
        }
        get { return _GameState; }
    }
    #endregion GameState

    #region Dificult
    internal enum Dificult
    {
        Easy,
        Medium,
        Hard
    }
    static Dificult _Dificult;
    internal static Dificult m_Dificult
    {
        set
        {
            if (_Dificult != value) _Dificult = value;
        }
        get { return _Dificult; }
    }
    #endregion Dificult
    #endregion Enums

    #region Events
    internal delegate void TickHandler(GameState _gameState);
    static event TickHandler _TickHandler = (_GameState) => { };
    internal static event TickHandler m_TickHandler
    {
        add
        {
            if (!_TickHandler.GetInvocationList().Contains(value))
            {
                _TickHandler += value;
            }
        }
        remove
        {
            if (_TickHandler.GetInvocationList().Contains(value))
            {
                _TickHandler -= value;
            }
        }
    }

    internal delegate void GameHandler(Dificult _dificult, GameState _gameState);
    static event GameHandler _GameHandler = (_dificult, _gameState) => { };
    internal static event GameHandler m_GameHandler
    {
        add
        {
            if (!_GameHandler.GetInvocationList().Contains(value))
            {
                _GameHandler += value;
            }
        }
        remove
        {
            if (_GameHandler.GetInvocationList().Contains(value))
            {
                _GameHandler -= value;
            }
        }
    }
    #endregion Events

    #region MonoBehavior
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform);
    }

    private void Update()
    {
        _TickHandler(_GameState);
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        DeInit();
    }

    void DeInit()
    {
        _TickHandler = (_GameState) => { };
        _GameHandler = (_dificule, _gameParameter) => { };
    }
    #endregion MonoBehavior
}
