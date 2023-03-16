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

    #region school project specific request types

    public class SendVerificationRequest : GoogleDriveRequest
    {
        public string email;
    }

    public class LoginRequest : GoogleDriveRequest
    {
        public string email;
        public string code;
    }

    public class SendSheetLinkRequest : GoogleDriveRequest
    {
        public string tableName;
        public string email;
    }

    #endregion

    #region Google Sheet request types

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

    #endregion

    #region test request types

    public class VerifyConnectionRequest : GoogleDriveRequest
    {
    }

    public class GetTestSheetRequest : GoogleDriveRequest
    {
    }

    #endregion


    [System.Serializable]
    public class Row
    {
        public List<string> cellValues = new List<string>();
    }
}

