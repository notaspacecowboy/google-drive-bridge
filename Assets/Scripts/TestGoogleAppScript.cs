using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GoogleDriveBridge;
using UnityEngine;
using UnityEngine.Networking;

public class TestGoogleAppScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ProcessRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void ProcessRequest()
    {
        var data = await GoogleDrive.Instance.SendVerificationCode("zyang464");
    }
}
