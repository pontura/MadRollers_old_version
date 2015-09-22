using UnityEngine;
using System.Collections;

public class Social : MonoBehaviour {

    static Social mInstance = null;

    public static Social Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a Social antes de inicializarse");
            }
            return mInstance;
        }
    }
    void Start()
    {
        mInstance = this;
        GetComponent<DataController>().Init();
    }
}
