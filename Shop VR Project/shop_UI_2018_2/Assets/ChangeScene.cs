using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class ChangeScene : MonoBehaviour {
    public void changeScene(int i)
    {
        bool success;
        success = VRTK_SDKManager.AttemptUnloadSDKSetup();
        if (success)
            Application.LoadLevel(i);
        else
            Debug.Log("not success");
    }
    public void changeScene(string i)
    {
        bool success;
        success = VRTK_SDKManager.AttemptUnloadSDKSetup();
        if (success)
            SceneManager.LoadScene(i);
        else
            Debug.Log("not success");
    }

    private void unload()
    {

    }
}
