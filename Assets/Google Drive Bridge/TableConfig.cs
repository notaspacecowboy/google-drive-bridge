using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GoogleDriveBridge;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;


namespace GoogleDriveBridge
{
    [Serializable]
    public enum ColumnType
    {
        String                 = 0,
        Integer                = 1,
        FloatingNumber         = 2,
    }

    [Serializable]
    public class GoogleSheetColumn
    {
        [SerializeField]
        [Tooltip("Please specify the name of your new column")]
        private string _columnName;

        public string ColumnName => _columnName;

        [SerializeField]
        [Tooltip("Please specify the type of data that will be stored in this column")]
        private ColumnType _dataType;

        public ColumnType DataType => _dataType;
    }

    [Serializable]
    [CreateAssetMenu(fileName = "TableConfig", menuName = "Google Drive Bridge/Table Configuration Data")]
    public class TableConfig : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Please specify the name of your google sheet table")]
        private string _tableName = "New Column";

        public string TableName => _tableName;

        [SerializeField]
        private List<GoogleSheetColumn> _allColumns;

        public List<GoogleSheetColumn> AllColumns => _allColumns;
    }






#if UNITY_EDITOR
    [CustomEditor(typeof(TableConfig))]
    public class TableConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);

            if (GUILayout.Button("Generate Class!"))
            {
                var tableConfig = (TableConfig)target;
                GenerateClass(tableConfig);
            }
        }


        private void GenerateClass(TableConfig config)
        {
            //compute generated class name
            string generatedClassName = config.TableName.Replace(" ", "");
            generatedClassName += "RowDefine";

            //get generated class string
            List<string> generatedClassStrings = new List<string>();
            generatedClassStrings.Add($"namespace GoogleDriveBridge {{");
            generatedClassStrings.Add($"    class {generatedClassName} {{");
            foreach (var column in config.AllColumns)
            {
                string type = GetColumnType(column.DataType);
                string fieldName = column.ColumnName.Replace(" ", "");
                generatedClassStrings.Add($"        public {type} {fieldName};");
            }

            //add a GetRow function to convert the generated class object to Row object
            generatedClassStrings.Add($"        public Row GetRow() {{");
            generatedClassStrings.Add($"            Row row = new Row();");
            foreach (var column in config.AllColumns)
            {
                string fieldName = column.ColumnName.Replace(" ", "");    //the work is done in compile time so performance is not important :)
                generatedClassStrings.Add($"            row.cellValues.Add({column.ColumnName}.ToString());");
            }
            generatedClassStrings.Add($"            return row;");
            generatedClassStrings.Add($"        }}");
            generatedClassStrings.Add($"    }}");
            generatedClassStrings.Add($"}}");


            //write generated class to file
            var path = AssetDatabase.GetAssetPath(config);
            var index = path.LastIndexOf('/');
            path = path.Remove(index + 1);
            File.WriteAllLines(path + generatedClassName + ".cs", generatedClassStrings);
            AssetDatabase.Refresh();
        }


        private string GetColumnType(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.String:
                    return "string";

                case ColumnType.Integer:
                    return "int";

                case ColumnType.FloatingNumber:
                    return "double";
            }

            return "string";
        }
    }
#endif

}

