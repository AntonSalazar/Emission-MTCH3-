using UnityEngine;
using System.Linq;

public class S_PlayerController : S_IniterObject
{
    #region External
    internal enum KeyState
    {
        Down,
        Up,
        Pressed
    }
    Event _Event;
    internal delegate void InputHandler(KeyCode _keyCode, KeyState _keyState);
    static event InputHandler _InputHandler = (_keyCode, _keyState) => { };
    internal static event InputHandler m_InputHandler
    {
        add
        {
            if (!_InputHandler.GetInvocationList().Contains(value)) _InputHandler += value;
        }
        remove
        {
            if (_InputHandler.GetInvocationList().Contains(value)) _InputHandler -= value;
        }
    }

    #region Inputs
    static Vector2 _Axis;
    internal static Vector2 m_Axis
    {
        get
        {
            _Axis.x = Input.GetAxis("Horizontal");
            _Axis.y = Input.GetAxis("Vertical");
            return _Axis;
        }
    }

    internal static bool m_Escape
    {
        get { return Input.GetKeyDown(KeyCode.Escape); }
    }

    internal static bool m_Enter
    {
        get { return Input.GetKeyDown(KeyCode.KeypadEnter); }
    }

    internal static bool m_Space
    {
        get { return Input.GetKeyDown(KeyCode.Space); }
    }

    internal static bool FireLeft
    {
        get { return Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.LeftAlt); }
    }

    internal static bool FireRight
    {
        get { return Input.GetKey(KeyCode.Mouse1); }
    }
    #endregion Inputs
    #endregion External

    #region MonoBehavior
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnGUI()
    {
        _Event = Event.current;
        switch (_Event.type)
        {
            case EventType.KeyDown:
                if (Input.GetKeyDown(_Event.keyCode)) _InputHandler(_Event.keyCode, KeyState.Down);
                break;
            case EventType.KeyUp:
                if (Input.GetKeyUp(_Event.keyCode)) _InputHandler(_Event.keyCode, KeyState.Up);
                break;
            default:
                if (Input.GetKey(_Event.keyCode)) _InputHandler(_Event.keyCode, KeyState.Pressed);
                break;
                
        }
    }
    #endregion MonoBehavior
}
