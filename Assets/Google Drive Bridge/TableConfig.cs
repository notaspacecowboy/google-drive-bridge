using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GoogleDriveBridge
{
    [Serializable]
    public enum ColumnType
    {
        String                 = 0,
        Number                 = 1,
        Date                   = 2,
        Dollar                 = 3,
    }

    [Serializable]
    public class GoogleSheetColumn
    {
        [SerializeField]
        [Tooltip("Please specify the name of your new column")]
        private string _columnName;

        [SerializeField]
        [Tooltip("Please specify the type of data that will be stored in this column")]
        private ColumnType _dataType;
    }

    [Serializable]
    [CreateAssetMenu(fileName = "TableConfig", menuName = "Google Drive Bridge/Table Configuration Data")]
    public class TableConfig : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Please specify the name of your google sheet table")]
        private string _tableName = "New Column";

        [SerializeField]
        private List<GoogleSheetColumn> _allColumns;
    }

}