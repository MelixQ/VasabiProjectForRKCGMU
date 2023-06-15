using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteHandler : MonoBehaviour
{
    [SerializeField] private string _noteContent = "";
    [SerializeField] private TMP_Text _noteText;
    [SerializeField] private TMP_Text _pageNumber;
    private static NoteHandler _instance;
    public static NoteHandler GetInstance() => _instance;
    private void Awake()
    {
        if (_instance is not null)
            Debug.LogWarning($"More than one instance of {this} found in the scene");
        _instance = this;
        
        SetupNoteText();
        UpdatePagination();
    }

    public void SetupNoteContent(string text)
    {
        _noteContent = text;
        SetupNoteText();
        UpdatePagination();
    }

    public void PreviousPage()
    {
        if (_noteText.pageToDisplay < 1)
        {
            _noteText.pageToDisplay = 1;
            return;
        }

        if (_noteText.pageToDisplay - 1 > 1)
            _noteText.pageToDisplay -= 1;
        else
            _noteText.pageToDisplay = 1;
        
        UpdatePagination();
    }

    public void NextPage()
    {
        if (_noteText.pageToDisplay >= _noteText.textInfo.pageCount)
            return;

        if (_noteText.pageToDisplay >= _noteText.textInfo.pageCount - 1)
            _noteText.pageToDisplay = _noteText.textInfo.pageCount - 1;

        else
            _noteText.pageToDisplay += 1;
        
        UpdatePagination();
    }

    private void UpdatePagination() => _pageNumber.text = _noteText.pageToDisplay.ToString();

    private void SetupNoteText() => _noteText.text = _noteContent;
}
