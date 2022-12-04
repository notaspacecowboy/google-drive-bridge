using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleDriveBridge
{  
    public class GoogleDriveRequestBase 
    {
        public int action;
        public string password;
        public string ssid;
    }

    public class VerifyConnectionRequest : GoogleDriveRequestBase
    {
    }

    public class SendVerificationRequest : GoogleDriveRequestBase
    {
        public string email;
    }

    public class LoginRequest : GoogleDriveRequestBase
    {
        public string email;
        public string code;
    }

    public class GetAllTablesRequest : GoogleDriveRequestBase
    {
    }

    public class CreateNewTableRequest : GoogleDriveRequestBase
    {
        public string tableName;
    }

    public class GetAllColumnsRequest : GoogleDriveRequestBase
    {
        public string tableName;
    }

    public class AppendRowRequest : GoogleDriveRequestBase
    {
        public string tableName;
        public List<string> cellValues;
    }

    public class AddNewColumnRequest : GoogleDriveRequestBase
    {
        public string tableName;
        public string columnName;
    }
}

