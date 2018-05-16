using UnityEngine;
using System.Collections.Generic;
public class S_Board : MonoBehaviour
{
    [SerializeField] Vector2Int _Board;
    [SerializeField] S_Gem _Gem;
    [SerializeField] AnimationCurve _GemMotion;

    static Vector2Int _SBoard;
    internal static AnimationCurve _SGemMotion;
    static List<S_Gem> _CurrentGems = new List<S_Gem>();
    static S_Gem[,] _Gems;

    private void OnEnable()
    {
        GM_Main.m_GameHandler += StartGame;
    }

    private void OnDisable()
    {
        GM_Main.m_GameHandler -= StartGame;
    }

    private void StartGame(GM_Main.Dificult _dificule, GM_Main.GameState _state)
    {
        if (_state == GM_Main.GameState.Gameplay)
        {
            _SBoard = _Board;
            _SGemMotion = _GemMotion;
            Init();
        }
    }

    private void Init()
    {
        _Gems = new S_Gem[_Board.y, _Board.x];
        for (int i = 0; i < _Board.y; i++)
        {
            for (int j = 0; j < _Board.x; j++)
            {
                S_Gem _gem = S_PoolManager.m_PoolManager.GetPoolObject(_Gem, new Vector3(j - (_Board.x / 2), i - (_Board.y / 2), transform.position.z), Quaternion.identity);
                _gem.m_Gem = (S_Gem.Gem)Random.Range(0, 5);
                _gem.transform.SetParent(transform);

                _Gems[i, j] = _gem;
                _gem._Sector = new Vector2Int(i, j);
            }
        }

        for (int i = 0; i < _Board.y; i++)
        {
            for (int j = 0; j < _Board.x; j++)
            {
                SearchNeighbors(i, j);

                if (i > 0 && i < _Board.y - 1)
                {
                    if (_Gems[i, j].m_Gem == _Gems[i - 1, j].m_Gem &&
                        _Gems[i, j].m_Gem == _Gems[i + 1, j].m_Gem)
                        _Gems[i, j].m_Gem = GenerateColor(_Gems[i, j].m_Gem);
                }

                if (j > 0 && j < _Board.x - 1)
                {
                    if (_Gems[i, j].m_Gem == _Gems[i, j - 1].m_Gem &&
                        _Gems[i, j].m_Gem == _Gems[i, j + 1].m_Gem)
                        _Gems[i, j].m_Gem = GenerateColor(_Gems[i, j].m_Gem);
                }
            }
        }
    }

    private S_Gem.Gem GenerateColor(S_Gem.Gem _current)
    {
        S_Gem.Gem _New = (S_Gem.Gem)Random.Range(0, 5);
        if (_New == _current) return GenerateColor(_current);

        return _New;
    }

    internal static void SearchNeighbors(Vector2Int _sector)
    {

        if (_sector.x > 0) _Gems[_sector.x, _sector.y].AddNeighbor(_Gems[_sector.x - 1, _sector.y]);
        if (_sector.x < _SBoard.y - 1) _Gems[_sector.x, _sector.y].AddNeighbor(_Gems[_sector.x + 1, _sector.y]);

        if (_sector.y > 0) _Gems[_sector.x, _sector.y].AddNeighbor(_Gems[_sector.x, _sector.y - 1]);
        if (_sector.y < _SBoard.x - 1) _Gems[_sector.x, _sector.y].AddNeighbor(_Gems[_sector.x, _sector.y + 1]);
    }

    internal static void SearchNeighbors(int _x, int _y)
    {
        if (_x > 0) _Gems[_x, _y].AddNeighbor(_Gems[_x - 1, _y]);
        if (_x < _SBoard.y - 1) _Gems[_x, _y].AddNeighbor(_Gems[_x + 1, _y]);

        if (_y > 0) _Gems[_x, _y].AddNeighbor(_Gems[_x, _y - 1]);
        if (_y < _SBoard.x - 1) _Gems[_x, _y].AddNeighbor(_Gems[_x, _y + 1]);
    }

    internal static void ClickedGem(S_Gem _gem)
    {
        _CurrentGems.Add(_gem);
        if (_CurrentGems.Count == 2)
        {
            if (_CurrentGems[0]._Neighbors.Contains(_CurrentGems[1]))
            {
                Vector2Int _tmpSector = _CurrentGems[0]._Sector;
                _CurrentGems[0]._Sector = _CurrentGems[1]._Sector;
                _CurrentGems[1]._Sector = _tmpSector;

                _Gems[_CurrentGems[0]._Sector.x, _CurrentGems[0]._Sector.y] = _CurrentGems[0];
                _Gems[_CurrentGems[1]._Sector.x, _CurrentGems[1]._Sector.y] = _CurrentGems[1];

                _CurrentGems[0].Clicked(_CurrentGems[1].m_Transform.position);
                _CurrentGems[1].Clicked(_CurrentGems[0].m_Transform.position);
            } 
            _CurrentGems.Clear();
        }
    }
}