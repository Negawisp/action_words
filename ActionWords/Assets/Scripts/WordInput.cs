﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordInput : MonoBehaviour
{
    private WordBook _book;

    private bool _inputMode = false;

    [SerializeField] private Text _text;
    [SerializeField]
    private List<TableCell> _selectedCells;

    private void Start()
    {
        _book = FindObjectOfType<WordBook>();
        _selectedCells = new List<TableCell>();
    }

    public bool InputModeEnabled() { return _inputMode; }
    public void EnableInputMode()  { _inputMode = true; }
    public void DisableInputMode() { _inputMode = false; }


    public void TakeLetter(TableCell cell)
    {
        _text.text = _text.text + cell.Letter();
        _selectedCells.Add(cell);
    }
    
    public void Activate ()
    {
        if (_book.Contains(_text.text.ToLower()))
        {
            foreach (TableCell cell in _selectedCells)
            {
                cell.Activate();
            }
        }
        else
        {
            foreach (TableCell cell in _selectedCells)
            {
                cell.Unselect();
            }
        }
        Clear();
    }


    private void Clear()
    {
        _text.text = "";
        _selectedCells.Clear();
        _inputMode = false;
    }
}
