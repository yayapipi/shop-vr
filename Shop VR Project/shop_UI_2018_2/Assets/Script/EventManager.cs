using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public delegate void ClickAction();
    public static event ClickAction updateUserData;
    private static users userData;
    private static shopitems[] shopItemsData;
    private static pics[] shopItemPics;
    private static shopcartitems[] shopCartItems;
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
        userData = data;
    }

    public static users GetUserData()
    {
        return userData;
    }

    public static void SetShopItemsData(shopitems[] data)
    {
        shopItemsData = data;
    }

    public static shopitems[] GetShopItemsData()
    {
        return shopItemsData;
    }

    public static void SetShopItemPics(pics[] data)
    {
        shopItemPics = data;
    }

    public static pics[] GetShopItemPics()
    {
        return shopItemPics;
    }

    public static void SetShopCartItems(shopcartitems[] data)
    {
        shopCartItems = data;
    }

    public static shopcartitems[] GetShopCartItems()
    {
        return shopCartItems;
    }

    public static void UpdateUserData()
    {
        if (updateUserData != null)
            updateUserData();
    }
}
