using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class ChangeScene : MonoBehaviour 
{
    public void changeScene(string i)
    {
        SteamVR_LoadLevel.Begin("Scene_room");
    }
}
