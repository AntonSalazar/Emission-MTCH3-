using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class S_Gem : S_PoolObject
{
    internal enum Gem
    {
        Red     = 0,
        Green   = 1,
        Blue    = 2,
        Yellow  = 3,
        Purpule = 4
    }

    [Header("Gem settigns")]
    [SerializeField, ReadOnly] internal Vector2Int _Sector;
    [SerializeField] Material[] _Materials;
    [SerializeField] internal List<S_Gem> _Neighbors;


    bool _Clicked;
    Vector3 _PrevPosition, _NextPosition;

    Gem _Gem;
    internal Gem m_Gem
    {
        set
        {
            _Gem = value;
            m_MeshRenderer.sharedMaterial = _Materials[(int)value];
        }
        get { return _Gem; }
    }

    bool g_MeshRenderer;
    MeshRenderer _MeshRenderer;
    MeshRenderer m_MeshRenderer
    {
        get
        {
            if (!g_MeshRenderer)
                if (_MeshRenderer = GetComponentInChildren<MeshRenderer>()) g_MeshRenderer = true;
            return _MeshRenderer;
        }
    }

    float _Timer;
    float m_Timer
    {
        set
        {
            _Timer = value;
            m_Transform.position = Vector3.Lerp(_PrevPosition, _NextPosition, S_Board._SGemMotion.Evaluate(_Timer));
            if (_Timer > 1.0f)
            {
                _Timer = 0.0f;
                _Clicked = false;
                _PrevPosition = _NextPosition;
            }
        }
        get { return _Timer; }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GM_Main.m_TickHandler += Tick;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GM_Main.m_TickHandler -= Tick;
    }

    private void OnMouseDown()
    {
        S_Board.ClickedGem(this);
    }

    internal void Clicked(Vector3 _position)
    {
        _Neighbors.Clear();
        S_Board.SearchNeighbors(_Sector);
        for (int i = 0; i < _Neighbors.Count; i++)
        {
            _Neighbors[i]._Neighbors.Clear();
            S_Board.SearchNeighbors(_Neighbors[i]._Sector);
        }

        _Clicked = true;
        _NextPosition = _position;
    }

    private void Start()
    {
        _PrevPosition = m_Transform.position;
    }


    private void Tick(GM_Main.GameState _gameState)
    {
        if (_gameState == GM_Main.GameState.Gameplay)
        {
            if (_Clicked)
            {
                m_Timer += Time.deltaTime * 2.5f;
            }
        }
    }

    internal void AddNeighbor(S_Gem _gem)
    {
        if (!_Neighbors.Contains(_gem)) _Neighbors.Add(_gem);
    }

    internal void RemoveNeighbor(S_Gem _gem)
    {
        if (_Neighbors.Contains(_gem)) _Neighbors.Remove(_gem);
    }

}
