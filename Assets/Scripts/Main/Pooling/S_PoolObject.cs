using UnityEngine;
using System.Collections;

public class S_PoolObject : MonoBehaviour
{
    #region External
    [Header("Pool Object Settings")]
    [SerializeField] protected bool _UseLifeTime = true;
    [SerializeField] protected float _LifeTime = 5.0f;

    bool _isActive;
    internal bool m_isActive
    {
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                if (_UseLifeTime)
                {
                    switch (_isActive)
                    {
                        case false:
                            StopAllCoroutines();
                            gameObject.SetActive(false);
                            break;
                        case true:
                            gameObject.SetActive(true);
                            StartCoroutine(_ILifeCicle());
                            break;
                    }
                }
                else { gameObject.SetActive(_isActive); }
            }
        }
        get { return _isActive; }
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
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        GM_Main.m_TickHandler += Tick;
    }

    protected virtual void OnDisable()
    {
        GM_Main.m_TickHandler -= Tick;
    }

    private IEnumerator _ILifeCicle()
    {
        yield return new WaitForSeconds(_LifeTime);
        m_isActive = false;
    }

    private void Tick(GM_Main.GameState _gameState)
    {
        LifeCicle(_gameState);
    }

    internal virtual void LifeCicle(GM_Main.GameState _gameState)
    {

    }
    #endregion MonoBehavior
}
