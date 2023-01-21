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
        public List<string> cellValues;
    }

    public class AddNewColumnRequest : GoogleDriveRequest
    {
        public string tableName;
        public string columnName;
    }
}

