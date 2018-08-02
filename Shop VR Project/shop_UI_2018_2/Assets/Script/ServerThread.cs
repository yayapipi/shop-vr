using System;
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
    private int user_id;
    private Action call;

    // The constructor obtains the state information and the
    // callback delegate.
    public GetUserDataThread(Action callbackDelegate)
    {
        sqlConnection = MainController.getSqlConnection();
        user_id = MainController.GetUserID();
        call = callbackDelegate;
    }

    // The thread procedure performs the task, such as
    // formatting and printing a document, and then invokes
    // the callback delegate.
    public void GetUserData()
    {
        users user_data = sqlConnection.getusers(user_id);
        //Console.WriteLine("Using thread to get userData", user_ID);

        EventManager.SetUserData(ref user_data);

        if(call != null)
            UnityMainThreadDispatcher.Instance().Enqueue(call);
    }
}

public class ShopThread
{
    private sqlapi sqlConnection;
    private int user_id;
    private int item_id;
    private int shopAmount;
    private Action call;

    public ShopThread(int item_ID, int amount, Action callbackDelegate)
    {
        sqlConnection = MainController.getSqlConnection();
        user_id = MainController.GetUserID();
        item_id = item_ID;
        shopAmount = amount;
        call = callbackDelegate;
    }

    public void Buy()
    {
        users user_data = sqlConnection.getusers(user_id);
        shopitems[] items = sqlConnection.getshop_item(1, item_id);
        //Console.WriteLine("Using thread to buy item");

        if (items.Length != 0)
        {
            double totalprice = items[0].cost * shopAmount;

            if (items[0].cost * shopAmount < user_data.money)
            {
                userinvent invent = sqlConnection.getuserinvent(user_data.id, items[0].id);
                if (invent.user_id > 0)
                    sqlConnection.Up_userinvent(user_data.id, items[0].id, shopAmount + invent.amount);
                else
                    sqlConnection.Add_userinvent(user_data.id, items[0].id, shopAmount);
                sqlConnection.Up_users(user_data.id, user_data.money - totalprice);

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

        if(call != null)
            UnityMainThreadDispatcher.Instance().Enqueue(call);
    }

    public void Cart()
    {
        shopitems[] items = sqlConnection.getshop_item(1, item_id);
        //Console.WriteLine("Using thread to buy item");

        if (items.Length != 0)
        {
            shopcart cart = sqlConnection.getshopcart(user_id, items[0].id);
            if (cart.user_id > 0)
                sqlConnection.Up_shopcart(user_id, items[0].id, shopAmount + cart.amount);
            else
                sqlConnection.Add_shopcart(user_id, items[0].id, shopAmount);
        }
        else
        {
            EventManager.SetMessage("item not found");
        }

        if (call != null)
            UnityMainThreadDispatcher.Instance().Enqueue(call);
    }
}