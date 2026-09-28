using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update()
    {
        //Debug.Log(isFirstUpdate);
        if (isFirstUpdate)
        {            
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
