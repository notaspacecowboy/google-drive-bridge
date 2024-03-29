using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Xml.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace GoogleDriveBridge
{
    [Serializable]
    [CreateAssetMenu(fileName = "GoogleDriveConfig", menuName = "Google Drive Bridge/Drive Configuration Data", order = 1)]
    public class GoogleDriveConfig : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Please specify the deployed url of your google apps script")]
        private string _url = "";

        public string Url => _url;

        [SerializeField] [Tooltip("Please specify the password of your google apps script")]
        private string _password = "";
        public string Password => _password;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GoogleDriveConfig))]
    public class GoogleDriveConfigEditor : Editor
    {
        private SerializedProperty _url;
        private SerializedProperty _password;

        private bool mIsConnectionVerified = false;
        private string mVerifyResult = "";

        private void OnEnable()
        {
            _url = serializedObject.FindProperty("_url");
            _password = serializedObject.FindProperty("_password");
        }

        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector())
            {
                mIsConnectionVerified = false;
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Verify Connection"))
            {
                mIsConnectionVerified = true;
                CheckConnection();   
            }

            if (mIsConnectionVerified)
            {
                GUILayout.Label(mVerifyResult);
            }
        }

        private async void CheckConnection()
        {
            string url = _url.stringValue;
            string password = _password.stringValue;
            Debug.Log($"url: {url}, password: {password}");


            VerifyConnectionRequest request = new VerifyConnectionRequest();
            request.password = password;
            request.action = (int) RequestCode.VerifyConnection;

            string jsonRequest = JsonUtility.ToJson(request);
            var response = await UnityWebRequest.Post(url, jsonRequest).SendWebRequest();
            if (response.result != UnityWebRequest.Result.Success)
            {
                mVerifyResult = "Connection failed!";
            }
            else
            {
                var json = response.downloadHandler.text;
                mVerifyResult = json;
            }
        }
    }
#endif
}
