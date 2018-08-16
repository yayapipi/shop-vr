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
    private static userinventory[] userInventoryData;
    private static string msgTitle;
    private static string msgText;

    public static void SetMessage(string title, string text)
    {
        msgTitle = title;
        msgText = text;
    }

    public static string GetMessage1()
    {
        return msgTitle;
    }

    public static string GetMessage2()
    {
        return msgText;
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

    public static void SetUserInventData(userinventory[] data)
    {
        userInventoryData = data;
    }

    public static userinventory[] GetUserInventData()
    {
        return userInventoryData;
    }
}
