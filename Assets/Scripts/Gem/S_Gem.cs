using UnityEngine;
using System.Linq;

public class S_Gem : S_PoolObject
{
    [Header("Gem ReadOnly")]
    [SerializeField, ReadOnly] GemColor _GemColor;
    [SerializeField, ReadOnly] bool _isMatched;
    [SerializeField, ReadOnly] bool _isClicked;
    [SerializeField, ReadOnly] Vector2Int _Sector;

    [Header("Settings")]
    [SerializeField] Material[] _Materials;
    [SerializeField] AnimationCurve _CurveMotion;

    Vector2Int _OldSector;

    internal enum GemColor
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        Purpule = 4
    }

    internal GemColor M_GemColor
    {
        set
        {
            _GemColor = value;
            M_MeshRenderer.sharedMaterial = _Materials[(int)value];

        }
        get { return _GemColor; }
    }

    internal bool M_isMatched
    {
        set
        {
            _isMatched = value;
            if (_isMatched)
            {
                m_isActive = false;
                M_isMatched = false;
                _OldSector = new Vector2Int(_OldSector.x, 5);
                S_Board.FindEmpty();
            }
        }
        get { return _isMatched; }
    }

    bool g_MeshRenderer;
    MeshRenderer _MeshRenderer;
    MeshRenderer M_MeshRenderer
    {
        get
        {
            if (!g_MeshRenderer)
                if (_MeshRenderer = GetComponentInChildren<MeshRenderer>()) g_MeshRenderer = true;
            return _MeshRenderer;
        }
    }
    
    internal Vector2Int M_Sector
    {
        set
        {
            _Sector = value;
        }
        get { return _Sector; }
    }

    float _Timer;
    float M_Timer
    {
        set
        {
            _Timer = value;
            Vector3 _OldPosition = new Vector3(_OldSector.x - (S_Board.M_sBoard.x / 2), _OldSector.y - (S_Board.M_sBoard.y / 2), m_Transform.position.z);
            Vector3 _NewPosition = new Vector3(M_Sector.x - (S_Board.M_sBoard.x / 2), M_Sector.y - (S_Board.M_sBoard.y / 2), m_Transform.position.z);
            m_Transform.position = Vector3.Lerp(_OldPosition, _NewPosition, _CurveMotion.Evaluate(_Timer));
            if (_Timer > 1.0f)
            {
                _Timer = 0.0f;
                _isClicked = false;
                _OldSector = M_Sector;
                _MatchFindHandler();
            }
        }
        get { return _Timer; }
    }

    delegate void MatchFindHandler();
    static event MatchFindHandler _MatchFindHandler = () => { };
    static event MatchFindHandler M_MatchFindHandler
    {
        add
        {
            if (!_MatchFindHandler.GetInvocationList().Contains(value)) _MatchFindHandler += value;
        }
        remove
        {
            if (_MatchFindHandler.GetInvocationList().Contains(value)) _MatchFindHandler -= value;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GM_Main.m_TickHandler += Tick;
        M_MatchFindHandler += FindMatches;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GM_Main.m_TickHandler -= Tick;
        M_MatchFindHandler -= FindMatches;
    }

    private void Start()
    {
        _OldSector = M_Sector;
    }

    private void OnMouseDown()
    {
        S_Board.Clicked(this);
    }

    private void Tick(GM_Main.GameState _state)
    {
        if (_state == GM_Main.GameState.Gameplay)
        {
            if (_isClicked)
            {
                M_Timer += Time.deltaTime * 2.5f;
            }
        }
    }

    internal void SetSector(bool _x, int _value)
    {
        S_Gem _Neighbor;
        Vector2Int _NewSector = new Vector2Int((_x) ? M_Sector.x + _value : M_Sector.x, (_x) ? M_Sector.y : M_Sector.y + _value);
        _Neighbor = S_Board.M_GemsGrid[_NewSector.x, _NewSector.y];
        _Neighbor.M_Sector = new Vector2Int((_x) ? _Neighbor.M_Sector.x - _value : _Neighbor.M_Sector.x, (_x) ? _Neighbor.M_Sector.y : _Neighbor.M_Sector.y - _value);
        M_Sector = _NewSector;
        S_Board.M_GemsGrid[M_Sector.x, M_Sector.y] = this;
        S_Board.M_GemsGrid[_Neighbor.M_Sector.x, _Neighbor.M_Sector.y] = _Neighbor;

        
        _isClicked = true;
        _Neighbor._isClicked = true;
    }

    private void FindMatches()
    {
        name = "Gem (" + M_Sector.x + " : " + M_Sector.y + ")";
        if (M_Sector.x > 0 && M_Sector.x < S_Board.M_sBoard.x - 1)
        {
            S_Gem _Left = S_Board.M_GemsGrid[M_Sector.x - 1, M_Sector.y];
            S_Gem _Right = S_Board.M_GemsGrid[M_Sector.x + 1, M_Sector.y];

            if (M_GemColor == _Left.M_GemColor && M_GemColor == _Right.M_GemColor)
            {
                M_isMatched = true;
                _Left.M_isMatched = true;
                _Right.M_isMatched = true;
            }
        }

        if (M_Sector.y > 0 && M_Sector.y < S_Board.M_sBoard.y - 1)
        {
            S_Gem _Down = S_Board.M_GemsGrid[M_Sector.x, M_Sector.y - 1];
            S_Gem _Up = S_Board.M_GemsGrid[M_Sector.x, M_Sector.y + 1];

            if (M_GemColor == _Down.M_GemColor && M_GemColor == _Up.M_GemColor)
            {
                M_isMatched = true;
                _Down.M_isMatched = true;
                _Up.M_isMatched = true;
            }
        }
    }
}
