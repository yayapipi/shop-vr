﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    public void changeScene(int i)
    {
        Application.LoadLevel(i);
    }
    public void changeScene(string i)
    {
        SceneManager.LoadScene(i);
    }
}
