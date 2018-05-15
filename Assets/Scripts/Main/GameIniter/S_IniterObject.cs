using UnityEngine;

public class S_IniterObject : MonoBehaviour
{
    #region External
    [SerializeField, ReadOnly]int _Hash;
    internal int m_Hash
    {
        set { _Hash = value; }
        get { return _Hash; }
    }

    static bool _isQuited;
    bool _isDestroyed;
    bool _isInited;
    internal bool m_isInited
    {
        private set
        {
            _isInited = value;
            g_Transform = false;

            switch (_isInited)
            {
                case false:
                    if (_isDestroyed) S_GameIniter.RemoveIniterObject(this);
                    else Invoke("SetActive", 0.01f);
                    break;

                case true:
                    S_GameIniter.AddIniterObject(this);
                    break;
            }
        }
        get { return _isInited; }
    }

    bool g_Transform;
    Transform _Transform;
    internal Transform m_Transform
    {
        get
        {
            if (!g_Transform)
                if (_Transform = transform) g_Transform = true;
            return _Transform;
        }
    }
    #endregion External

    #region MonoBehavior
    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {
        m_isInited = true;
        Debug.Log("<color=green> InitedObject: " + gameObject.name + " has INITED!</color>");
    }

    protected virtual void OnDisable()
    {
        if (!_isQuited)
        {
            m_isInited = false;
            Debug.Log("<color=yellow> InitedObject: " + gameObject.name + " has DEINITED! RESTART initialization...</color> ");
        }
    }

    private void SetActive()
    {
        gameObject.SetActive(true);
    }

    protected virtual void OnDestroy()
    {
        if (!_isQuited)
        {
            _isDestroyed = true;
            m_isInited = false;
            Debug.Log("<color=red> InitedObject: " + gameObject.name + " has DESTROYED! RESTART initialization...</color>");
        }
       
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuited = true;
    }

#endregion MonoBehavior
}
