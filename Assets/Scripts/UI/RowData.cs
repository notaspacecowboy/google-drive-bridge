using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowData : MonoBehaviour
{
    [SerializeField]
    private InputField _rowValueInput;

    public string RowValue
    {
        get
        {
            return _rowValueInput.text;
        }
    }


    [SerializeField]
    private Text _columnNameText;

    public string ColumnNameText
    {
        get
        {
            return _columnNameText.text;
        }
        set
        {
            _columnNameText.text = value;
        }
    }
}
