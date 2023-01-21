using System.Collections;
using System.Collections.Generic;
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

        #region APIs

        public void SetUserSheetId(string sheetId)
        {
            _sid = sheetId;
        }

        public async UniTask<ResponseData> VerifyConnection()
        {
            VerifyConnectionRequest request = new VerifyConnectionRequest();
            return await ProcessRequest(request, RequestCode.VerifyConnection);
        }

        public async UniTask<ResponseData> SendVerificationCode(string email)
        {
            SendVerificationRequest request = new SendVerificationRequest() {email = email};
            return await ProcessRequest(request, RequestCode.SendVerificationCode);
        }


        public async UniTask<ResponseData> Login(string email, string verificationCode)
        {
            LoginRequest request = new LoginRequest() {email = email, code = verificationCode};
            return await ProcessRequest(request, RequestCode.Login);
        }


        public async UniTask<ResponseData> GetAllTables()
        {
            GetAllTablesRequest request = new GetAllTablesRequest();
            return await ProcessRequest(request, RequestCode.GetAllTables);
        }


        public async UniTask<ResponseData> CreateNewTable(string tableName)
        {
            CreateNewTableRequest request = new CreateNewTableRequest() {tableName = tableName};
            return await ProcessRequest(request, RequestCode.CreateNewTable);
        }



        public async UniTask<ResponseData> GetAllColumnsOfTable(string tableName)
        {
            GetAllColumnsRequest request = new GetAllColumnsRequest() {tableName = tableName};
            return await ProcessRequest(request, RequestCode.GetAllColumnsOfTable);
        }


        public async UniTask<ResponseData> AppendRow(string tableName, List<string> cellValues)
        {
            AppendRowRequest request = new AppendRowRequest() {tableName = tableName, cellValues = cellValues};
            return await ProcessRequest(request, RequestCode.AppendRow);
        }


        public async UniTask<ResponseData> AddNewColumn(string tableName, string columnName)
        {
            AddNewColumnRequest request = new AddNewColumnRequest() {tableName = tableName, columnName = columnName};
            return await ProcessRequest(request, RequestCode.AddNewColumn);
        }

        #endregion

        #region private fields

        [SerializeField]
        [InspectorName("Configuration Data")]
        private GoogleDriveConfig _config;

        private string _sid;

        #endregion

        #region private methods

        private void FillForm(GoogleDriveRequest request, RequestCode code)
        {
            int requestCode = (int)code;

            request.password = _config.Password;
            request.action = requestCode;

            if (requestCode > (int)RequestCode.Login)
                request.ssid = _sid;
        }

        private async UniTask<ResponseData> ProcessRequest(GoogleDriveRequest request, RequestCode code)
        {
            FillForm(request, code);
            string jsonRequest = JsonUtility.ToJson(request);

            var json = (await UnityWebRequest.Post(_config.Url, jsonRequest).SendWebRequest()).downloadHandler.text;
            Debug.Log(json);
            return JsonUtility.FromJson<ResponseData>(json);
        }

        #endregion
    }
}
