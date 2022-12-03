using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T mInstance;


    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
         
        DontDestroyOnLoad(mInstance);
    }

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<T>();
                if (mInstance == null)
                {
                    Debug.LogError($"Google Drive Instance does not exist, make sure you put at least one instance in the scene");
                    return null;
                }
            }

            return mInstance;
        }
    }

    /// <summary>
    /// for monosingleton derived classes, do not implement Awake function, implement this instead
    /// </summary>
    protected virtual void OnAwake()
    {

    }
}
