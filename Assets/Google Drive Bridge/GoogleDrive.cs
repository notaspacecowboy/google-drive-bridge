using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;



namespace GoogleDriveBridge
{
    public enum RequestCode
    {
        VerifyConnection                   = 0,
        SendVerificationCode               = 1,
        Login                              = 2,
        GetAllTables                       = 3,
        CreateNewTable                     = 4,
        GetAllColumnsOfTable               = 5,
        AppendRow                          = 6,
        AddNewColumn                       = 7,
    }


    public class GoogleDrive : MonoBehaviour
    {
        #region singleton

        private static GoogleDrive _instance;


        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(_instance);
        }

        public static GoogleDrive Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GoogleDrive>();
                    if (_instance == null)
                    {
                        Debug.LogError($"Google Drive Instance does not exist, make sure you put at least one instance in the scene");
                        return null;
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region request json class

        private class GoogleDriveRequestBase
        {
            public int action;
            public string password;
            public string ssid;
        }

        private class AppendRowRequest: GoogleDriveRequestBase
        {
            public string tableName;
            public List<string> cellValues;
        }

        private class AddNewColumnRequest : GoogleDriveRequestBase
        {
            public string tableName;
            public string columnName;
        }

        #endregion

        #region APIs

        public void SetUserSheetId(string sheetId)
        {
            _config.Sid = sheetId;
        }

        public async UniTask<ResponseData> SendVerificationCode(string email)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();

            FillForm(form, RequestCode.SendVerificationCode);
            form.Add("email", email);

            return await ProcessRequest(form);
        }


        public async UniTask<ResponseData> Login(string email, string verificationCode)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();

            FillForm(form, RequestCode.Login);
            form.Add("email", email);
            form.Add("code", verificationCode);

            return await ProcessRequest(form);
        }


        public async UniTask<ResponseData> GetAllTables()
        {
            Dictionary<string, string> form = new Dictionary<string, string>();

            FillForm(form, RequestCode.GetAllTables);

            return await ProcessRequest(form);
        }


        public async UniTask<ResponseData> CreateNewTable(string tableName)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();

            FillForm(form, RequestCode.CreateNewTable);
            form.Add("tableName", tableName);

            return await ProcessRequest(form);
        }



        public async UniTask<ResponseData> GetAllColumnsOfTable(string tableName)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();

            FillForm(form, RequestCode.GetAllColumnsOfTable);
            form.Add("tableName", tableName);

            return await ProcessRequest(form);
        }


        public async UniTask<ResponseData> AppendRow(string tableName, List<string> cellValues)
        {
            AppendRowRequest request = new AppendRowRequest();
            FillForm(request, RequestCode.AppendRow);
            request.tableName = tableName;
            request.cellValues = new List<string>(cellValues);

            string jsonRequest = JsonUtility.ToJson(request);
            return await ProcessRequest(jsonRequest);
        }


        public async UniTask<ResponseData> AddNewColumn(string tableName, string columnName)
        {
            AddNewColumnRequest request = new AddNewColumnRequest();
            FillForm(request, RequestCode.AddNewColumn);
            request.tableName = tableName;
            request.columnName = columnName;

            string jsonRequest = JsonUtility.ToJson(request);
            return await ProcessRequest(jsonRequest);
        }

        #endregion

        #region private fields

        [SerializeField]
        [InspectorName("Configuration Data")]
        private GoogleDriveConfig _config;

        #endregion

        #region private methods

        private void FillForm(Dictionary<string, string> form, RequestCode code)
        {
            form.Add("password", _config.Password);
            int requestCode = (int)code;
            form.Add("action", requestCode.ToString());

            if (requestCode > (int)RequestCode.Login)
                form.Add("ssid", _config.Sid);
        }

        private void FillForm(GoogleDriveRequestBase request, RequestCode code)
        {
            int requestCode = (int)code;

            request.password = _config.Password;
            request.action = requestCode;

            if (requestCode > (int)RequestCode.Login)
                request.ssid = _config.Sid;
        }

        private async UniTask<ResponseData> ProcessRequest(Dictionary<string, string> form)
        {
            var json = (await UnityWebRequest.Post(_config.Url, form).SendWebRequest()).downloadHandler.text;
            Debug.Log(json);
            return JsonUtility.FromJson<ResponseData>(json);
        }

        private async UniTask<ResponseData> ProcessRequest(string jsonData) {
            var json = (await UnityWebRequest.Post(_config.Url, jsonData).SendWebRequest()).downloadHandler.text;
            Debug.Log(json);
            return JsonUtility.FromJson<ResponseData>(json);
        }

        #endregion
    }
}
