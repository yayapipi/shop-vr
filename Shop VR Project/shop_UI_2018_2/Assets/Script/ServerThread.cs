﻿using System;
using System.Collections;
using System.Threading;

// The ThreadWithState class contains the information needed for
// a task, the method that executes the task, and a delegate
// to call when the task is complete.
//
public class GetUserDataThread
{
    // State information used in the task.
    private sqlapi sqlConnection;
    private int userID;
    private Action callbackDelegate;

    // The constructor obtains the state information and the
    // callback delegate.
    public GetUserDataThread(Action callbackDelegate)
    {
        sqlConnection = MainController.getSqlConnection();
        userID = MainController.GetUserID();
        this.callbackDelegate = callbackDelegate;
    }

    // The thread procedure performs the task, such as
    // formatting and printing a document, and then invokes
    // the callback delegate.
    public void GetUserData()
    {
        users userData = sqlConnection.getusers(userID);
        //Console.WriteLine("Using thread to get userData", userID);

        EventManager.SetUserData(ref userData);

        if(callbackDelegate != null)
            UnityMainThreadDispatcher.Instance().Enqueue(callbackDelegate);
    }
}

public class ShopThread
{
    private sqlapi sqlConnection;
    private int userID;
    private int itemID;
    private int amount;
    private Action callbackDelegate;

    public ShopThread(int itemID, int amount, Action callbackDelegate)
    {
        sqlConnection = MainController.getSqlConnection();
        userID = MainController.GetUserID();
        this.itemID = itemID;
        this.amount = amount;
        this.callbackDelegate = callbackDelegate;
    }

    public void Buy()
    {
        users userData = sqlConnection.getusers(userID);
        shopitems[] items = sqlConnection.getshop_item(1, itemID);
        //Console.WriteLine("Using thread to buy item");

        if (items.Length != 0)
        {
            double totalprice = items[0].cost * amount;

            if (items[0].cost * amount < userData.money)
            {
                userinvent invent = sqlConnection.getuserinvent(userData.id, items[0].item_id);
                if (invent.user_id > 0)
                    sqlConnection.Up_userinvent(userData.id, items[0].item_id, amount + invent.amount);
                else
                    sqlConnection.Add_userinvent(userData.id, items[0].item_id, amount);
                sqlConnection.Up_users(userData.id, userData.money - totalprice);

                EventManager.SetMessage("Thank you");
            }
            else
            {
                EventManager.SetMessage("Not enough $$");
            }
        }
        else
        {
            EventManager.SetMessage("item not found");
        }

        if(callbackDelegate != null)
            UnityMainThreadDispatcher.Instance().Enqueue(callbackDelegate);
    }

    public void Cart()
    {
        shopitems[] items = sqlConnection.getshop_item(1, itemID);
        //Console.WriteLine("Using thread to buy item");

        if (items.Length != 0)
        {
            shopcart cart = sqlConnection.getshopcart(userID, items[0].item_id);
            if (cart.user_id > 0)
                sqlConnection.Up_shopcart(userID, items[0].item_id, amount);
            else
                sqlConnection.Add_shopcart(userID, items[0].item_id, amount);
        }
        else
        {
            EventManager.SetMessage("item not found");
        }

        if (callbackDelegate != null)
            UnityMainThreadDispatcher.Instance().Enqueue(callbackDelegate);
    }
}

public class GetShopItemsThread
{
    private sqlapi sqlConnection;
    private int shopID;
    private int type;
    private string orderby;
    private string sc;
    private int limitbase;
    private int limitoffset;
    private IEnumerator callbackEnumerator;

    public GetShopItemsThread(int shopID, int type, string orderby, string sc, int limitbase, int limitoffset, IEnumerator callbackEnumerator)
    {
        sqlConnection = MainController.getSqlConnection();
        this.shopID = shopID;
        this.type = type;
        this.orderby = orderby;
        this.sc = sc;
        this.limitbase = limitbase;
        this.limitoffset = limitoffset;
        this.callbackEnumerator = callbackEnumerator;
    }

    public void GetShopItems()
    {
        shopitems[] shopItemsData;

        if (type == 0)
            shopItemsData = sqlConnection.Rshop_item(shopID, orderby, sc, limitbase, limitoffset);
        else
            shopItemsData = sqlConnection.Rshop_item(shopID, type, orderby, sc, limitbase, limitoffset);

        //Console.WriteLine("Using thread to get shop items");

        EventManager.SetShopItemsData(shopItemsData);

        if (callbackEnumerator != null)
            UnityMainThreadDispatcher.Instance().Enqueue(callbackEnumerator);
    }
}

public class GetShopItemPicsThread
{
    private sqlapi sqlConnection;
    private int itemID;
    private IEnumerator callbackEnumerator;

    public GetShopItemPicsThread(int itemID, IEnumerator callbackEnumerator)
    {
        sqlConnection = MainController.getSqlConnection();
        this.itemID = itemID;
        this.callbackEnumerator = callbackEnumerator;
    }

    public void GetShopItemPics()
    {
        pics[] shopItemPics = sqlConnection.Ritem_pic(itemID, "id", "asc", 0, 100);

        //Console.WriteLine("Using thread to get shop item pics");

        EventManager.SetShopItemPics(shopItemPics);

        if (callbackEnumerator != null)
            UnityMainThreadDispatcher.Instance().Enqueue(callbackEnumerator);
    }
}

public class GetShopCartItemsThread
{
    private sqlapi sqlConnection;
    private int userID;
    private IEnumerator callbackEnumerator;

    public GetShopCartItemsThread(IEnumerator callbackEnumerator)
    {
        sqlConnection = MainController.getSqlConnection();
        this.userID = MainController.GetUserID();
        this.callbackEnumerator = callbackEnumerator;
    }

    public void GetShopCartItems()
    {
        shopcartitems[] shopCartItems = sqlConnection.Rshopcartitems(userID);

        //Console.WriteLine("Using thread to get shop cart items");

        EventManager.SetShopCartItems(shopCartItems);

        if (callbackEnumerator != null)
            UnityMainThreadDispatcher.Instance().Enqueue(callbackEnumerator);
    }
}

