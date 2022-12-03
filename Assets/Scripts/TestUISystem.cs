using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GoogleDriveBridge;
using UnityEngine;

public class TestUISystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.Init();
        UIManager.Instance.PushAsync<LoginPanel>().Forget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
