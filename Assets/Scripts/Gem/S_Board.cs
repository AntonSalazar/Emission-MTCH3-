using UnityEngine;
using System.Collections.Generic;

public class S_Board : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] Vector2Int _Board;
    [SerializeField] S_Gem _GemObject;
    [SerializeField, Range(0, 5)] int _Dificult = 5;

    static int _sDificult;
    static List<S_Gem> _ClickedGems = new List<S_Gem>();
    static S_Gem[,] _GemsGrid;
    internal static S_Gem[,] M_GemsGrid
    {
        private set { _GemsGrid = value; }
        get { return _GemsGrid; }
    }
    static Vector2Int _sBoard;
    internal static Vector2Int M_sBoard
    {
        private set { _sBoard = value; }
        get { return _sBoard; }
    }

    private void Awake()
    {
        M_sBoard = _Board;
    }

    private void OnEnable()
    {
        GM_Main.m_GameHandler += StartGame;
    }

    private void OnDisable()
    {
        GM_Main.m_GameHandler -= StartGame;
    }

    private void StartGame(GM_Main.Dificult _dificult, GM_Main.GameState _gameState)
    {
        if (_gameState == GM_Main.GameState.Gameplay)
        {
            _sDificult = _Dificult;
            GenerateGrid();
            GenerateColor();
        }
    }

    private void GenerateGrid()
    {
        M_GemsGrid = new S_Gem[M_sBoard.x, M_sBoard.y];

        for (int i = 0; i < M_sBoard.x; i++)
        {
            for (int j = 0; j < M_sBoard.y; j++)
            {
                S_Gem _newGem = S_PoolManager.m_PoolManager.GetPoolObject(_GemObject, new Vector3(i - (M_sBoard.x / 2), j - (M_sBoard.y / 2), transform.position.z), Quaternion.identity);
                _newGem.name = "Gem (" + i + " : " + j + ")";
                _newGem.m_Transform.SetParent(transform);
                _newGem.M_Sector = new Vector2Int(i, j);
                _newGem.M_GemColor = (S_Gem.GemColor)Random.Range(0, _Dificult);
                M_GemsGrid[i, j] = _newGem;
            }
        }
    }

    private void GenerateColor()
    {
        for (int i = 0; i < M_sBoard.x; i++)
        {
            for (int j = 0; j < M_sBoard.y; j++)
            {
                if (i > 0 && i < M_sBoard.x - 1)
                {
                    if (M_GemsGrid[i, j].M_GemColor == M_GemsGrid[i - 1, j].M_GemColor &&
                        M_GemsGrid[i, j].M_GemColor == M_GemsGrid[i + 1, j].M_GemColor)
                        M_GemsGrid[i, j].M_GemColor = GenerateColor(M_GemsGrid[i, j].M_GemColor);
                }

                if (j > 0 && j < M_sBoard.y - 1)
                {
                    if (M_GemsGrid[i, j].M_GemColor == M_GemsGrid[i, j - 1].M_GemColor &&
                        M_GemsGrid[i, j].M_GemColor == M_GemsGrid[i, j + 1].M_GemColor)
                        M_GemsGrid[i, j].M_GemColor = GenerateColor(M_GemsGrid[i, j].M_GemColor);
                }
            }
        }
    }

    private static S_Gem.GemColor GenerateColor(S_Gem.GemColor _current)
    {
        S_Gem.GemColor _New = (S_Gem.GemColor)Random.Range(0, _sDificult);
        if (_New == _current) return GenerateColor(_current);

        return _New;
    }

    internal static void Clicked(S_Gem _gem)
    {
        _ClickedGems.Add(_gem);
        if (_ClickedGems.Count == 2)
        {
            Vector2Int _Result = _ClickedGems[0].M_Sector - _ClickedGems[1].M_Sector;

            if (_Result.x == -1) _ClickedGems[0].SetSector(true, 1);        // right
            else if (_Result.x == 1) _ClickedGems[0].SetSector(true, -1);   // left
            else if (_Result.y == -1) _ClickedGems[0].SetSector(false, 1);  // up
            else if (_Result.y == 1) _ClickedGems[0].SetSector(false, -1);  // down
            _ClickedGems.Clear();
        }
    }

    internal static void FindEmpty()
    {
        List<S_Gem> _Empty = new List<S_Gem>();
        for (int i = 0; i < M_sBoard.x; i++)
        {
            for (int j = 0; j < M_sBoard.y; j++)
            {
                if (!M_GemsGrid[i, j].m_isActive && !_Empty.Contains(M_GemsGrid[i, j])) _Empty.Add(M_GemsGrid[i, j]);
                if (!M_GemsGrid[i, j].m_isActive && j < M_sBoard.y - 1)
                {
                    M_GemsGrid[i, j].M_GemColor = GenerateColor(M_GemsGrid[i, j].M_GemColor);
                    M_GemsGrid[i, j].SetSector(false, 1);
                }
            }
        }

        for (int i = 0; i < _Empty.Count;i++)
        {
            _Empty[i].m_isActive = true;
        }
    }
}
