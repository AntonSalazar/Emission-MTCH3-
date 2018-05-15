
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
#endif

using UnityEngine;
using System.Collections.Generic;

public class S_GameIniter : MonoBehaviour
{
    #region External
    [SerializeField] S_IniterObject[] _IniterPrefabs;
    static List<S_IniterObject> _OnScene = new List<S_IniterObject>();
    static bool _isQuited;
    #endregion External

    #region Inited
    bool _isDestroyed;
    bool _isInited;
    internal bool m_isInited
    {
        private set
        {
            _isInited = value;
            switch (_isInited)
            {
                case false:
                    if (_isDestroyed)
                    {
                        GameObject _NewGameIniter = Instantiate(Resources.Load("Initer/" + SceneManager.GetActiveScene().name + "/" + "S_GameIniter", typeof(GameObject))) as GameObject;
                        _NewGameIniter.name = gameObject.name;
                    }
                    else Invoke("SetActive", 0.1f);
                    break;
            }
        }
        get { return _isInited; }
    }
    #endregion Inited

    #region MonoBehavior
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        S_IniterObject[] _InScene = FindObjectsOfType<S_IniterObject>();
        for (int i = 0; i < _IniterPrefabs.Length; i++)
        {
            bool _isFounded = false;
            for (int j = 0; j < _InScene.Length; j++)
            {
                if (_InScene[j].m_Hash == _IniterPrefabs[i].m_Hash)
                {
                    AddIniterObject(_InScene[j]);
                    _isFounded = true;
                    break;
                }
            }
            if (!_isFounded)
            {
                S_IniterObject _obj = Instantiate(_IniterPrefabs[i], Vector3.zero, Quaternion.identity);
                _obj.name = _IniterPrefabs[i].name;
            }
        }
    }

    private void SetActive()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        m_isInited = true;
        Debug.Log("<color=green> INITER: " + gameObject.name + " HAS ACTIVE!</color>");
    }

    private void OnDisable()
    {
        if (!_isQuited)
        {
            m_isInited = false;
            Debug.Log("<color=yellow> INITER: " + gameObject.name + " HAS DEACTIVATED! RESTART INITIALIZATION... </color>");
        }
    }

    private void OnDestroy()
    {
        if (!_isQuited)
        {
            _isDestroyed = true;
            m_isInited = false;
            Debug.Log("<color=red> INITER: " + gameObject.name + " HAS DESTROYED! RESTART INITIALIZATION... </color>");
        }
    }

    private void OnApplicationQuit()
    {
        _isQuited = true;
    }

    internal static void AddIniterObject(S_IniterObject _initerObject)
    {
        if (!_OnScene.Contains(_initerObject))
        {
            _OnScene.Add(_initerObject);
        }
    }

    internal static void RemoveIniterObject(S_IniterObject _initerObject)
    {
        if (_OnScene.Contains(_initerObject) && !_isQuited)
        {
            _OnScene.Remove(_initerObject);

            S_IniterObject _obj = Instantiate(_initerObject, _initerObject.transform.position, _initerObject.transform.rotation);
            _obj.name = _initerObject.name;
            _obj.gameObject.SetActive(true);
        }
    }

    #endregion MonoBehavior

    #region Editor

#if UNITY_EDITOR
    [ContextMenu("Create Initer")]
    private void CreateIniter()
    {
        Directory.CreateDirectory(Application.dataPath + "/Resources/Initer/" + SceneManager.GetActiveScene().name);
        AssetDatabase.Refresh();
        PrefabUtility.CreatePrefab("Assets/Resources/Initer/" + SceneManager.GetActiveScene().name +"/"+ "GameIniter.prefab", gameObject);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            for (int i = 0; i < _IniterPrefabs.Length; i++)
            {
                if (_IniterPrefabs[i].m_Hash != i)
                {
                    _IniterPrefabs[i].m_Hash = i;
                    S_IniterObject[] _InScene = FindObjectsOfType<S_IniterObject>();
                    for (int j = 0; j < _InScene.Length; j++)
                    {
                        if (_IniterPrefabs[i].GetType() == _InScene[j].GetType())
                        {
                            PrefabUtility.RevertPrefabInstance(_InScene[j].gameObject);
                            break;
                        }
                    }
                }
            }
        }
    }
#endif
    #endregion Editor
}
