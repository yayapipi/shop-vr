using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public delegate void ClickAction();
    public static event ClickAction updateUserData;
    private static users user_data;
    private static string msg;

    public static void SetMessage(string message)
    {
        msg = message;
    }

    public static string GetMessage()
    {
        return msg;
    }

    public static void SetUserData(ref users data)
    {
        user_data = data;
    }

    public static users GetUserData()
    {
        return user_data;
    }

    public static void UpdateUserData()
    {
        if (updateUserData != null)
            updateUserData();
    }
}
