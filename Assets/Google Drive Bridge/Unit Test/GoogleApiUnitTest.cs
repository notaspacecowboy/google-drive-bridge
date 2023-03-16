using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace GoogleDriveBridge
{
    public class GoogleApiUnitTest
    {
        public static async UniTask RunTests()
        {
            var sid = GoogleDrive.Instance.UserSheetID;

            var currentTables = new List<string>() {"Sheet1"};
            var currentColumns = new List<string>();

            await Test_VerifyConnection();
            await Test_GetTestSheet();

            await Test_GetAllTables(currentTables);

            await Test_CreateNewTable(currentTables);
            await Test_GetAllTables(currentTables);


            var currentTable = currentTables[1];
            await Test_GetAllColumnsOfTable(currentTable, currentColumns);

            await Test_AddNewColumn(currentTable, currentColumns);
            await Test_GetAllColumnsOfTable(currentTable, currentColumns);

            await Test_AppendRow(currentTable, currentColumns);
            await Test_AddRows(currentTable, currentColumns);

            GoogleDrive.Instance.UserSheetID = sid;
        }

        private static async UniTask Test_VerifyConnection()
        {
            var response = await GoogleDrive.Instance.VerifyConnection();
            if (response == null || !response.result)
            {
                Debug.LogError($"VerifyConnection failed");
                return;
            }
        }

        private static async UniTask Test_GetTestSheet()
        {
            var response = await GoogleDrive.Instance.GetTestSheet();
            if (response == null || !response.result)
            {
                Debug.LogError($"SetupTestSheet failed");
                return;
            }

            GoogleDrive.Instance.UserSheetID = response.sid;
        }

        private static async UniTask Test_GetAllTables(List<string> correctTables)
        {
            var response = await GoogleDrive.Instance.GetAllTables();
            if (response == null || !response.result)
            {
                Debug.LogError($"Test_GetAllTables failed");
                return;
            }

            if (response.tables.Length != correctTables.Count)
            {
                Debug.LogError($"Test_GetAllTables failed");
                return;
            }

            for (int i = 0; i < response.tables.Length; i++)
            {
                if (response.tables[i] != correctTables[i])
                {
                    Debug.LogError($"Test_GetAllTables failed");
                    return;
                }
            }
        }

        private static async UniTask Test_CreateNewTable(List<string> correctTables)
        {
            TableID++;
            correctTables.Add($"test{TableID}");
            var response = await GoogleDrive.Instance.CreateNewTable($"test{TableID}");
            if (response == null || !response.result)
            {
                Debug.LogError($"Test_CreateNewTable failed");
                return;
            }
        }

        private static async UniTask Test_GetAllColumnsOfTable(string tableName, List<string> allColumns)
        {
            var response = await GoogleDrive.Instance.GetAllColumnsOfTable(tableName);
            if (response == null || !response.result)
            {
                Debug.LogError($"Test_GetAllColumnsOfTable failed");
                return;
            }

            if (response.columnName.Length != allColumns.Count)
            {
                Debug.LogError($"Test_GetAllColumnsOfTable failed");
                return;
            }

            for (int i = 0; i < response.columnName.Length; i++)
            {
                if (response.columnName[i] != allColumns[i])
                {
                    Debug.LogError($"Test_GetAllColumnsOfTable failed");
                    return;
                }
            }
        }


        private static async UniTask Test_AddNewColumn(string tableName, List<string> allColumns)
        {
            ColumnID++;

            allColumns.Add($"column{ColumnID}");
            var response = await GoogleDrive.Instance.AddNewColumn(tableName, $"column{ColumnID}");
            if (response == null || !response.result)
            {
                Debug.LogError($"Test_AddNewColumn failed");
                return;
            }
        }


        private static async UniTask Test_AppendRow(string tableName, List<string> allColumns)
        {
            RowID++;
            Row row = new Row();
            foreach (var column in allColumns)
            {
                row.cellValues.Add($"row{RowID}");
            }


            var response = await GoogleDrive.Instance.AppendRow(tableName, row);
            if (response == null || !response.result)
            {
                Debug.LogError($"Test_AppendRow failed");
                return;
            }
        }


        private static async UniTask Test_AddRows(string tableName, List<string> allColumns)
        {
            List<Row> rows = new List<Row>();
            for (int i = 0; i < 5; i++)
            {
                RowID++;
                Row row = new Row();
                foreach (var column in allColumns)
                {
                    row.cellValues.Add($"row{RowID}");
                }
                rows.Add(row);
            }


            var response = await GoogleDrive.Instance.AddRows(tableName, rows);
            if (response == null || !response.result)
            {
                Debug.LogError($"Test_AddRows failed");
                return;
            }
        }



        private static int TableID = 0;
        private static int ColumnID = 0;
        private static int RowID = 0;
    }
}
