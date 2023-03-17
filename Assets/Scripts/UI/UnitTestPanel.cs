using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GoogleDriveBridge;
using UnityEngine;
using UnityEngine.UI;

public class UnitTestPanel : UIPanel
{
    [SerializeField]
    private Button _unitTestButton;

    [SerializeField]
    private LoadingSpinner _loadingSpinner;

    [SerializeField]
    private GameObject _unitTestItemPrefab;

    [SerializeField]
    private Transform _resultRoot;

    private List<GameObject> _allResults = new List<GameObject>();


    private int _tableID = 0;
    private int _columnID = 0;
    private int _rowID = 0;

    public override async UniTask InitAsync()
    {
        DisableInput();

        CanvasGroup.alpha = 0;
        DisableInput();

        await CanvasGroup.DOFade(1, 3).SetEase(Ease.InOutCubic);

        EnableInput();
    }

    public override void EnableInput()
    {
        base.EnableInput();

        _unitTestButton.interactable = true;
    }

    public override void DisableInput()
    {
        base.DisableInput();

        _unitTestButton.interactable = false;
    }
    
    public void OnUnitTestButtonClicked()
    {
        RunUnitTest().Forget();
    }


    private void AddResult(string request, bool result)
    {
        var go = Instantiate(_unitTestItemPrefab);
        var item = go.GetComponent<UnitTestItem>();
        item.SetAPIName(request);
        item.SetResult(result);
        _allResults.Add(go);

        go.transform.parent = _resultRoot;
        go.transform.localEulerAngles = Vector3.zero;
    }

    private async UniTask RunUnitTest()
    {
        DisableInput();
        _loadingSpinner.Show();

        //unit test setup
        foreach (var go in _allResults)
            Destroy(go);
        _allResults.Clear();
        var sid = GoogleDrive.Instance.UserSheetID;
        var currentTables = new List<string>() { "Sheet1" };
        var currentColumns = new List<string>();

        //unit test calls
        var result = await Test_VerifyConnection();
        AddResult("VerifyConnection", result);

        result = await Test_GetTestSheet();
        AddResult("GetTestSheet", result);

        result = await Test_GetAllTables(currentTables);
        AddResult("GetAllTables", result);

        result = await Test_CreateNewTable(currentTables);
        result = await Test_GetAllTables(currentTables);
        AddResult("CreateNewTable", result);


        var currentTable = currentTables[1];
        result = await Test_GetAllColumnsOfTable(currentTable, currentColumns);
        AddResult("GetAllColumnsOfTable", result);

        result = await Test_AddNewColumn(currentTable, currentColumns);
        result = await Test_GetAllColumnsOfTable(currentTable, currentColumns);
        AddResult("AddNewColumn", result);

        result = await Test_AppendRow(currentTable, currentColumns);
        AddResult("AppendRow", result);

        result = await Test_AddRows(currentTable, currentColumns);
        AddResult("AddRows", result);

        _loadingSpinner.Hide();
        EnableInput();
    }


    #region unit tests

    private async UniTask<bool> Test_VerifyConnection()
    {
        var response = await GoogleDrive.Instance.VerifyConnection();
        if (response == null || !response.result)
        {
            Debug.Log($"VerifyConnection failed");
            return false;
        }

        return true;
    }

    private async UniTask<bool> Test_GetTestSheet()
    {
        var response = await GoogleDrive.Instance.GetTestSheet();
        if (response == null || !response.result)
        {
            Debug.LogError($"SetupTestSheet failed");
            return false;
        }

        GoogleDrive.Instance.UserSheetID = response.sid;
        return true;
    }

    private async UniTask<bool> Test_GetAllTables(List<string> correctTables)
    {
        var response = await GoogleDrive.Instance.GetAllTables();
        if (response == null || !response.result)
        {
            Debug.LogError($"Test_GetAllTables failed");
            return false;
        }

        if (response.tables.Length != correctTables.Count)
        {
            Debug.LogError($"Test_GetAllTables failed");
            return false;
        }

        for (int i = 0; i < response.tables.Length; i++)
        {
            if (response.tables[i] != correctTables[i])
            {
                Debug.LogError($"Test_GetAllTables failed");
                return false;
            }
        }

        return true;
    }

    private async UniTask<bool> Test_CreateNewTable(List<string> correctTables)
    {
        _tableID++;
        correctTables.Add($"test{_tableID}");
        var response = await GoogleDrive.Instance.CreateNewTable($"test{_tableID}");
        if (response == null || !response.result)
        {
            Debug.LogError($"Test_CreateNewTable failed");
            return false;
        }

        return true;
    }

    private async UniTask<bool> Test_GetAllColumnsOfTable(string tableName, List<string> allColumns)
    {
        var response = await GoogleDrive.Instance.GetAllColumnsOfTable(tableName);
        if (response == null || !response.result)
        {
            Debug.LogError($"Test_GetAllColumnsOfTable failed");
            return false;
        }

        if (response.columnName.Length != allColumns.Count)
        {
            Debug.LogError($"Test_GetAllColumnsOfTable failed");
            return false;
        }

        for (int i = 0; i < response.columnName.Length; i++)
        {
            if (response.columnName[i] != allColumns[i])
            {
                Debug.LogError($"Test_GetAllColumnsOfTable failed");
                return false;
            }
        }

        return true;
    }
    private async UniTask<bool> Test_AddNewColumn(string tableName, List<string> allColumns)
    {
        _columnID++;

        allColumns.Add($"column{_columnID}");
        var response = await GoogleDrive.Instance.AddNewColumn(tableName, $"column{_columnID}");
        if (response == null || !response.result)
        {
            Debug.LogError($"Test_AddNewColumn failed");
            return false;
        }

        return true;
    }


    private async UniTask<bool> Test_AppendRow(string tableName, List<string> allColumns)
    {
        _rowID++;
        Row row = new Row();
        foreach (var column in allColumns)
        {
            row.cellValues.Add($"row{_rowID}");
        }


        var response = await GoogleDrive.Instance.AppendRow(tableName, row);
        if (response == null || !response.result)
        {
            Debug.LogError($"Test_AppendRow failed");
            return false;
        }

        return true;
    }


    private async UniTask<bool> Test_AddRows(string tableName, List<string> allColumns)
    {
        List<Row> rows = new List<Row>();
        for (int i = 0; i < 5; i++)
        {
            _rowID++;
            Row row = new Row();
            foreach (var column in allColumns)
            {
                row.cellValues.Add($"row{_rowID}");
            }
            rows.Add(row);
        }


        var response = await GoogleDrive.Instance.AddRows(tableName, rows);
        if (response == null || !response.result)
        {
            Debug.LogError($"Test_AddRows failed");
            return false;
        }

        return true;
    }

    #endregion
}
