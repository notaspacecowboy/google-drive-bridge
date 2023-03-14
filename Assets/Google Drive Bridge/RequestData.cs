using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleDriveBridge
{  
    public class GoogleDriveRequest
    {
        public int action;
        public string password;
        public string ssid;
    }

    public class VerifyConnectionRequest : GoogleDriveRequest
    {
    }

    public class SendVerificationRequest : GoogleDriveRequest
    {
        public string email;
    }

    public class LoginRequest : GoogleDriveRequest
    {
        public string email;
        public string code;
    }

    public class GetAllTablesRequest : GoogleDriveRequest
    {
    }

    public class CreateNewTableRequest : GoogleDriveRequest
    {
        public string tableName;
    }

    public class GetAllColumnsRequest : GoogleDriveRequest
    {
        public string tableName;
    }

    public class AppendRowRequest : GoogleDriveRequest
    {
        public string tableName;
        public Row row;
    }

    public class AddNewColumnRequest : GoogleDriveRequest
    {
        public string tableName;
        public string columnName;
    }

    public class AddRowsRequest : GoogleDriveRequest
    {
        public string tableName;
        public bool clearBeforeWrite;         //if true, the user table will be cleared first before new rows are added
        public List<Row> rows;
    }


    public class SendSheetLinkRequest : GoogleDriveRequest
    {
        public string tableName;
        public string email;
    }

    [System.Serializable]
    public class Row
    {
        public List<string> cellValues = new List<string>();
    }
}

