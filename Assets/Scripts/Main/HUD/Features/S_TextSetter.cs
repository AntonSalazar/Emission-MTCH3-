using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class S_TextSetter : MonoBehaviour
{
    #region External
    [SerializeField] int _DefaultIndex = 0;
    [SerializeField] bool _UseRandomizer;
    [SerializeField, TextArea] string[] _NoteTexts = new string[1] {"Default Note"};
    Text _Text;
    #endregion External

    #region MonoBehavior
    private void Awake()
    {
        _Text = GetComponent<Text>();
        if (_UseRandomizer) SetRandomNoteText();
        else SetNoteText(_DefaultIndex);
    }

    internal void SetNoteText(int _index = 0)
    {
        if (_index < _NoteTexts.Length) _Text.text = _NoteTexts[_index];
    }

    internal void SetRandomNoteText()
    {
        if (_UseRandomizer) _Text.text = _NoteTexts[Random.Range(0, _NoteTexts.Length)];
    }
    #endregion MonoBehavior
}
