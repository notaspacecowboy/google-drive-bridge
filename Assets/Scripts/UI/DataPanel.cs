using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GoogleDriveBridge;
using UnityEngine;
using UnityEngine.UI;


public class DataPanel : UIPanel
{
    [SerializeField]
    private InputField _newTableInput;

    [SerializeField]
    private Button _newTableButton;

    [SerializeField]
    private Dropdown _tableNameSelector;

    [SerializeField]
    private Button _tableNameSelectButton;

    [SerializeField]
    private InputField _newColumnInput;

    [SerializeField]
    private Button _newColumnButton;

    [SerializeField]
    private Transform _rowRoot;

    [SerializeField]
    private GameObject _rowPrefab;

    [SerializeField]
    private Button _newRowButton;

    private List<RowData> _allRows = new List<RowData>();

    public override async UniTask InitAsync()
    {
        DisableInput();

        CanvasGroup.alpha = 0;
        DisableInput();

        await GetAllTables();

        await CanvasGroup.DOFade(1, 3).SetEase(Ease.InOutCubic);

        EnableInput();
    }

    public override void EnableInput()
    {
        base.EnableInput();

        _newTableButton.interactable = true;
        _newColumnButton.interactable = true;
        _tableNameSelectButton.interactable = true;
        _newRowButton.interactable = true;
    }

    public override void DisableInput()
    {
        base.DisableInput();

        _newTableButton.interactable = false;
        _newColumnButton.interactable = false;
        _tableNameSelectButton.interactable = false;
        _newRowButton.interactable = false;
    }


    public void OnAddNewTableButtonClicked()
    {
        AddNewTable().Forget();
    }

    public void OnTableSelected()
    {
        GetAllColumns().Forget();
    }

    public void OnAddNewColumnButtonClicked()
    {
        AddNewColumn().Forget();
    }

    public void OnAddNewRowButtonClicked()
    {
        AppendRow().Forget();
    }


    private async UniTask GetAllTables()
    {
        var response = await GoogleDrive.Instance.GetAllTables();
        _tableNameSelector.ClearOptions();
        List<Dropdown.OptionData> allTables = new List<Dropdown.OptionData>();
        foreach (var tableName in response.tables)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(tableName);
            allTables.Add(data);
        }
        _tableNameSelector.AddOptions(allTables);
    }

    private async UniTask AddNewTable()
    {
        DisableInput();

        string tableName = _newTableInput.text;
        if (tableName == "")
            return;

        var response = await GoogleDrive.Instance.CreateNewTable(tableName);
        if (response.result)
            await GetAllTables();

        EnableInput();
    }

    private async UniTask GetAllColumns()
    {
        DisableInput();

        string selectedTableName = _tableNameSelector.options[_tableNameSelector.value].text;
        var response = await GoogleDrive.Instance.GetAllColumnsOfTable(selectedTableName);

        if (!response.result)
            return;

        foreach (var data in _allRows)
        {
            Destroy(data.gameObject);
        }
        _allRows.Clear();
        
        foreach (var column in response.columnName)
        {
            var go = Instantiate(_rowPrefab);
            var data = go.GetComponent<RowData>();
            data.ColumnNameText = column;
            _allRows.Add(data);

            go.transform.parent = _rowRoot;
        }

        EnableInput();
    }


    private async UniTask AddNewColumn()
    {
        DisableInput();

        string selectedTableName = _tableNameSelector.options[_tableNameSelector.value].text;
        string columnName = _newColumnInput.text;

        await GoogleDrive.Instance.AddNewColumn(selectedTableName, columnName);
        await GetAllColumns();

        EnableInput();
    }


    private async UniTask AppendRow()
    {
        DisableInput();

        List<string> cellValues = new List<string>();
        foreach (var data in _allRows)
        {
            cellValues.Add(data.RowValue);
        }

        string selectedTableName = _tableNameSelector.options[_tableNameSelector.value].text;
        await GoogleDrive.Instance.AppendRow(selectedTableName, cellValues);

        EnableInput();
    }
}
