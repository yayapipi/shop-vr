using UnityEngine;
using System;
using System.Data;
using System.Collections;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;

public struct items
{
    public int id;
    public string name;
    public string pic_url;
    public int main_type;
    public float sub_type;
    public string description;
    public bool enabled;
    public string model_name;
    public string model_linkurl;
    public string standard_scale;
    public double panel_scale;
    public string created_at;
    public string updated_at;

};

public struct shopitems
{
    //items
    //public int id;
    public string name;
    public string pic_url;
    public int main_type;
    public float sub_type;
    public string description;
    public bool enabled;
    public string model_name;
    public string model_linkurl;
    public string standard_scale;
    public double panel_scale;
    public string created_at;
    public string updated_at;

    //shop_items
    public int shop_id;
    public int item_id;
    public int cost;
    public bool enable;
    public int click_times;
    public string s_created_at;
    public string s_updated_at;
};

public struct pics
{
    public int id;
    public int item_id;
    public string pic_url;
    public string created_at;
    public string updated_at;

};

public struct users
{
    public int id;
    public string name;
    public string email;
    public string pic_linkurl;
    public double money;
    public string world_asset_link;
    public string user_model_link;
    public int authority;
};

public struct userinvent
{
    public int user_id;
    public int item_id;
    public int amount;
    public bool locked;
    public string created_at;
    public string updated_at;
};

public struct shopcart
{
    public int user_id;
    public int item_id;
    public int amount;
    public string created_at;
    public string updated_at;
};

public struct shopcartitems
{
    //shop_carts
    public int user_id;
    //public int item_id;
    public int amount;
    public string c_created_at;
    public string c_updated_at;

    //items
    //public int id;
    public string name;
    public string pic_url;
    public int main_type;
    public float sub_type;
    public string description;
    public bool enabled;
    public string model_name;
    public string model_linkurl;
    public string standard_scale;
    public double panel_scale;
    public string created_at;
    public string updated_at;

    //shop_items
    public int shop_id;
    public int item_id;
    public int cost;
    public bool enable;
    public int click_times;
    public string s_created_at;
    public string s_updated_at;
}

public struct shops
{
    public int id;
    public string created_at;
};

public struct userinventory
{
    //user_inventories
    public int user_id;
    public int item_id;
    public int amount;
    public bool locked;
    public string created_at;
    public string updated_at;

    //items
    //public int id;
    public string name;
    public string pic_url;
    public int main_type;
    public float sub_type;
    public string description;
    public bool enabled;
    public string model_name;
    public string model_linkurl;
    public string standard_scale;
    public double panel_scale;
    //public string created_at;
    //public string updated_at;
};

public class sqlapi
{
    SqlAccess sql;
    string Error = null;

    public sqlapi()
    {
        sql = new SqlAccess();
    }

    public void closeSql()
    {
        sql.Close();
    }

    //private string[,] colum = new string[,]{{ "id", "name", "type", "price", "inventories", "description", "enabled", "added_time",
    //    "model_linkurl", "created_at", "updated_at"}};

    public users getusers(int ids)
    {
        users vvv = new users();
        string tables = "users";
        try
        {
            Debug.Log("sqlapi: " + "tables,id:" + tables + "," + ids);
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "id" }, new string[] { "=" }, new string[] { ids.ToString() },
                new string[] { }, new string[] { }, new string[] { }, new string[] { }).Tables[0];
            if (RT != null)
            {
                vvv.id = Convert.ToInt32(RT.Rows[0][0]);
                vvv.name = RT.Rows[0][1].ToString();
                vvv.email = RT.Rows[0][2].ToString();
                vvv.pic_linkurl = RT.Rows[0][4].ToString();
                vvv.money = Convert.ToDouble(RT.Rows[0][5]);
                vvv.world_asset_link = RT.Rows[0][6].ToString();
                vvv.user_model_link = RT.Rows[0][7].ToString();
                vvv.authority = Convert.ToInt32(RT.Rows[0][8]);
            }
            else
            {
                vvv.id = -1;
                Debug.Log("NAME NOT FOUND");
            }

            //sql.Close();
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public userinvent getuserinvent(int userid, int itemid)
    {
        userinvent vvv = new userinvent();
        string tables = "user_inventories";
        try
        {
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "user_id","item_id" }, new string[] { "=","=" }, new string[] { userid.ToString(),itemid.ToString() },
                new string[] { }, new string[] { }, new string[] { }, new string[] { }).Tables[0];
            if (RT != null)
            {
                vvv.user_id = Convert.ToInt32(RT.Rows[0][0]);
                vvv.item_id = Convert.ToInt32(RT.Rows[0][1]);
                vvv.amount = Convert.ToInt32(RT.Rows[0][2]);
                vvv.locked = Convert.ToBoolean(RT.Rows[0][3]);
                vvv.created_at = RT.Rows[0][4].ToString();
                vvv.updated_at = RT.Rows[0][5].ToString();
            }
            else
            {
                vvv.user_id = -1;
                vvv.item_id = -1;
                vvv.amount = -1;
                Debug.Log("NAME NOT FOUND");
            }
            //sql.Close();
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public userinventory[] Ruser_invent(int userid,int type, int limitbase, int limitoffset)
    {
        userinventory[] vvv = new userinventory[50];
        string tables = "user_inventories, items";
        try
        {
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "user_inventories.user_id", "items.id" }, new string[] {"=", "="}, new string[] { userid.ToString()
                , "user_inventories.item_id" }, new string[] { }, new string[] { }, new string[] { limitbase.ToString() }, new string[] { limitoffset.ToString() }).Tables[0];
            int i = 0;
            foreach (DataRow row in RT.Rows)
            {
                if (row != null)
                {
                    vvv[i].user_id = Convert.ToInt32(row["user_id"]);
                    vvv[i].item_id = Convert.ToInt32(row["item_id"]);
                    vvv[i].amount = Convert.ToInt32(row["amount"]);
                    vvv[i].locked = Convert.ToBoolean(row["locked"]);
                    vvv[i].created_at = row["created_at"].ToString();
                    vvv[i].updated_at = row["updated_at"].ToString();

                    vvv[i].name = row["name"].ToString();
                    vvv[i].pic_url = row["pic_url"].ToString();
                    vvv[i].main_type = Convert.ToInt32(row["main_type"]);
                    vvv[i].sub_type = (float)Convert.ToDouble(row["sub_type"]);
                    vvv[i].description = row["description"].ToString();
                    vvv[i].enabled = Convert.ToBoolean(row["enabled"]);
                    vvv[i].model_name = row["model_name"].ToString();
                    vvv[i].model_linkurl = row["model_linkurl"].ToString();
                    vvv[i].standard_scale = row["standard_scale"].ToString();
                    vvv[i].panel_scale = Convert.ToDouble(row["panel_scale"]);
                }
                else
                {
                    Debug.Log("id NOT FOUND");
                    break;
                }
                i++;
            }
            Array.Resize<userinventory>(ref vvv, i); //array(vvv)的大小設成counter(i)的大小
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }

        return vvv;
    }

    public shopcart getshopcart(int userid, int itemid)
    {
        shopcart vvv = new shopcart();
        string tables = "shop_carts";
        try
        {
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "user_id", "item_id" }, new string[] { "=", "=" }, new string[] { userid.ToString(), itemid.ToString() },
                new string[] { }, new string[] { }, new string[] { }, new string[] { }).Tables[0];
            if (RT != null)
            {
                vvv.user_id = Convert.ToInt32(RT.Rows[0][0]);
                vvv.item_id = Convert.ToInt32(RT.Rows[0][1]);
                vvv.amount = Convert.ToInt32(RT.Rows[0][2]);
                vvv.created_at = RT.Rows[0][4].ToString();
                vvv.updated_at = RT.Rows[0][5].ToString();
            }
            else
            {
                vvv.user_id = -1;
                vvv.item_id = -1;
                vvv.amount = -1;
                Debug.Log("NAME NOT FOUND");
            }
            //sql.Close();
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }
    
    public shopcartitems[] Rshopcartitems(int userid)
    {
        shopcartitems[] vvv = new shopcartitems[50];
        string tables = "shop_carts, shop_items, items";
        try
        {
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "shop_carts.user_id", "items.id", "shop_items.item_id" }, new string[] { "=", "=", "=" }, new string[] { userid.ToString(), "shop_carts.item_id", "shop_carts.item_id" },
                new string[] { }, new string[] { }, new string[] { }, new string[] { }).Tables[0];

            int i = 0;

            foreach (DataRow row in RT.Rows)
            {
                if (row != null)
                {
                    vvv[i].user_id = Convert.ToInt32(row["user_id"]);
                    //vvv[i].item_id = Convert.ToInt32(row["item_id"]);
                    vvv[i].amount = Convert.ToInt32(row["amount"]);
                    vvv[i].c_created_at = row["created_at"].ToString();
                    vvv[i].c_updated_at = row["updated_at"].ToString();

                    vvv[i].name = row["name"].ToString();
                    vvv[i].pic_url = row["pic_url"].ToString();
                    vvv[i].main_type = Convert.ToInt32(row["main_type"]);
                    vvv[i].sub_type = (float)Convert.ToDouble(row["sub_type"]);
                    vvv[i].description = row["description"].ToString();
                    vvv[i].enabled = Convert.ToBoolean(row["enabled"]);
                    vvv[i].model_name = row["model_name"].ToString();
                    vvv[i].model_linkurl = row["model_linkurl"].ToString();
                    vvv[i].standard_scale = row["standard_scale"].ToString();
                    vvv[i].panel_scale = Convert.ToDouble(row["panel_scale"]);
                    vvv[i].created_at = row["created_at2"].ToString();
                    vvv[i].updated_at = row["updated_at2"].ToString();

                    vvv[i].shop_id = Convert.ToInt32(row["shop_id"]);
                    vvv[i].item_id = Convert.ToInt32(row["item_id"]);
                    vvv[i].cost = Convert.ToInt32(row["cost"]);
                    vvv[i].enable = Convert.ToBoolean(row["enable"]);
                    vvv[i].click_times = Convert.ToInt32(row["click_times"]);
                    vvv[i].s_created_at = row["created_at1"].ToString();
                    vvv[i].s_updated_at = row["updated_at1"].ToString();
                    i++;
                }
                else
                {
                    Debug.Log("NOT FOUND");
                    break;
                }
            }

            //sql.Close();
            Array.Resize<shopcartitems>(ref vvv, i);
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public items getitems(int ids)
    {
        items vvv = new items();
        string tables = "items";
        //Debug.Log("getitemsaa");
        try
        {
            Debug.Log("sqlapi: " + "tables,id:" + tables + "," + ids);
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "id" }, new string[] { "=" }, new string[] { ids.ToString() },
                 new string[] { }, new string[] { }, new string[] { }, new string[] { }).Tables[0];
            if (RT != null)
            {
                //Debug.Log("aa"+RT.Rows[0][0].ToString());
                vvv.id = Convert.ToInt32(RT.Rows[0][0]);
                vvv.name = RT.Rows[0][1].ToString();
                vvv.pic_url = RT.Rows[0][2].ToString();
                vvv.main_type = Convert.ToInt32(RT.Rows[0][3]);
                vvv.sub_type = (float)Convert.ToDouble(RT.Rows[0][4]);
                vvv.description = RT.Rows[0][5].ToString();
                vvv.enabled = Convert.ToBoolean(RT.Rows[0][6]);
                vvv.model_name = RT.Rows[0][7].ToString();
                vvv.model_linkurl = RT.Rows[0][8].ToString();
                vvv.standard_scale = RT.Rows[0][9].ToString();
                vvv.panel_scale = Convert.ToDouble(RT.Rows[0][10]);
                vvv.created_at = RT.Rows[0][10].ToString();
                vvv.updated_at = RT.Rows[0][11].ToString();
                //    Debug.Log("id=" + RT.Rows[0][10].GetType());

            }
            else
            {
                vvv.id = -1;
                Debug.Log("NOT FOUND");
            }

            //sql.Close();
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public pics getpics(int ids)
    {
        pics vvv = new pics();
        string tables = "pics";
        try
        {
            Debug.Log("sqlapi: " + "tables,id:" + tables + "," + ids);
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "id" }, new string[] { "=" }, new string[] { ids.ToString() },
                 new string[] { }, new string[] { }, new string[] { }, new string[] { }).Tables[0];
            if (RT != null)
            {
                vvv.id = Convert.ToInt32(RT.Rows[0][0]);
                vvv.item_id = Convert.ToInt32(RT.Rows[0][1].ToString());
                vvv.pic_url = RT.Rows[0][2].ToString();
            }
            else
            {
                vvv.id = -1;
                Debug.Log("NAME NOT FOUND");
            }

            //sql.Close();
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public pics[] Ritem_pic(int ids, string orderby, string sc, int limitbase, int limitoffset)
    {
        //輸入items表的id,回傳對應pic值
        /*orderby:根據什麼欄位排序
        sc:順序(asc)or逆序(desc)
        limitbase:從第幾個id開始
        limitoffset:要取幾個值
        比如要取id從2到7的值:就是limitbase=1,limitoffset=7*/
        pics[] vvv = new pics[100];
        string tables = "pics";
        try
        {
            //Debug.Log("sqlapi: " + "tables,id:" + tables + "," + ids);
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "pics.item_id" }, new string[] { "=" }, new string[] { ids.ToString() },
                 new string[] { orderby }, new string[] { sc }, new string[] { limitbase.ToString() }, new string[] { limitoffset.ToString() }).Tables[0];
            int i = 0;
            foreach (DataRow row in RT.Rows)
            {
                if (row != null)
                {
                    vvv[i].id = Convert.ToInt32(row["id"]);
                    vvv[i].item_id = Convert.ToInt32(row["item_id"]);
                    vvv[i++].pic_url = row["pic_url"].ToString();
                }
                else
                {
                    Debug.Log("NAME NOT FOUND");
                    break;
                }
            }
            //Debug.Log("i=" + i); //counter
            //sql.Close();
            Array.Resize<pics>(ref vvv, i); //array(vvv)的大小設成counter(i)的大小
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public shopitems[] getshop_item(int shopid, int itemid)
    {
        shopitems[] vvv = new shopitems[100];
        string tables = "shop_items,items";
        try
        {
            Debug.Log("sqlapi: " + "shopid,itemid:" + shopid + "," + itemid);
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "shop_items.shop_id", "shop_items.item_id", "items.id" }, new string[] { "=", "=","=" }, new string[] { shopid.ToString(), itemid.ToString(), itemid.ToString() },
                 new string[] {  }, new string[] {  }, new string[] { }, new string[] {}).Tables[0];
            int i = 0;
            foreach (DataRow row in RT.Rows)
            {
                if (row != null)
                {
                    //vvv[i].id = Convert.ToInt32(row["id"]);
                    vvv[i].name = row["name"].ToString();
                    vvv[i].pic_url = row["pic_url"].ToString();
                    vvv[i].main_type = Convert.ToInt32(row["main_type"]);
                    vvv[i].sub_type = (float)Convert.ToDouble(row["sub_type"]);
                    vvv[i].description = row["description"].ToString();
                    vvv[i].enabled = Convert.ToBoolean(row["enabled"]);
                    vvv[i].model_name = row["model_name"].ToString();
                    vvv[i].model_linkurl = row["model_linkurl"].ToString();
                    vvv[i].standard_scale = row["standard_scale"].ToString();
                    vvv[i].panel_scale = Convert.ToDouble(row["panel_scale"]);
                    vvv[i].created_at = row["created_at1"].ToString();
                    vvv[i].updated_at = row["updated_at1"].ToString();

                    vvv[i].shop_id = Convert.ToInt32(row["shop_id"]);
                    vvv[i].item_id = Convert.ToInt32(row["item_id"]);
                    vvv[i].cost = Convert.ToInt32(row["cost"]);
                    vvv[i].enable = Convert.ToBoolean(row["enable"]);
                    vvv[i].click_times = Convert.ToInt32(row["click_times"]);
                    vvv[i].s_created_at = row["created_at"].ToString();
                    vvv[i].s_updated_at = row["updated_at"].ToString();
                    i++;
                }
                else
                {
                    Debug.Log("id NOT FOUND");
                    break;
                }
            }
            //sql.Close();
            Array.Resize<shopitems>(ref vvv, i); //array(vvv)的大小設成counter(i)的大小
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public shopitems[] Rshop_item(int ids, string orderby, string sc, int limitbase, int limitoffset)
    {
        /*ids:輸入shopid,回傳對應同商店的items值(物品資料)
         orderby:根據什麼欄位排序
         sc:順序(asc)or逆序(desc)
         limitbase:從第幾個id開始
         limitoffset:要取幾個值
         比如要取id從2到7的值:就是limitbase=1,limitoffset=7*/
        shopitems[] vvv = new shopitems[100];
        string tables = "shop_items,items";
        try
        {
            //Debug.Log("sqlapi: " + "tables,id:" + tables + "," + ids);
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "shop_items.shop_id", "items.id" }, new string[] { "=", "=" }, new string[] { ids.ToString(), "shop_items.item_id" },
                 new string[] { orderby }, new string[] { sc }, new string[] { limitbase.ToString() }, new string[] { limitoffset.ToString() }).Tables[0];
            int i = 0;
            int length = RT.Rows.Count;
            foreach (DataRow row in RT.Rows)
            {
                //Debug.Log("id:"+ row["id"] + "cost:"+ row["cost"] + "pic_url:"+ row["pic_url"].ToString());
                if (row != null)
                {
                    //vvv[i].id = Convert.ToInt32(row["id"]);
                    vvv[i].name = row["name"].ToString();
                    vvv[i].pic_url = row["pic_url"].ToString();
                    vvv[i].main_type = Convert.ToInt32(row["main_type"]);
                    vvv[i].sub_type = (float)Convert.ToDouble(row["sub_type"]);
                    vvv[i].description = row["description"].ToString();
                    vvv[i].enabled = Convert.ToBoolean(row["enabled"]);
                    vvv[i].model_name = row["model_name"].ToString();
                    vvv[i].model_linkurl = row["model_linkurl"].ToString();
                    vvv[i].standard_scale = row["standard_scale"].ToString();
                    vvv[i].panel_scale = Convert.ToDouble(row["panel_scale"]);
                    vvv[i].created_at = row["created_at1"].ToString();
                    vvv[i].updated_at = row["updated_at1"].ToString();

                    vvv[i].shop_id = Convert.ToInt32(row["shop_id"]);
                    vvv[i].item_id = Convert.ToInt32(row["item_id"]);
                    vvv[i].cost = Convert.ToInt32(row["cost"]);
                    vvv[i].enable = Convert.ToBoolean(row["enable"]);
                    vvv[i].click_times = Convert.ToInt32(row["click_times"]);
                    vvv[i].s_created_at = row["created_at"].ToString();
                    vvv[i].s_updated_at = row["updated_at"].ToString();
                    i++;
                }
                else
                {
                    Debug.Log("NAME NOT FOUND");
                    break;
                }
            }
            //Debug.Log("i=" + i); //counter
            //Debug.Log("col=" + RT.Columns.Count);
            //Debug.Log("row=" + RT.Rows.Count);
            //sql.Close();
            Array.Resize<shopitems>(ref vvv, i); //array(vvv)的大小設成counter(i)的大小
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    public shopitems[] Rshop_item(int ids, int type, string orderby, string sc, int limitbase, int limitoffset)
    {
        /*ids:輸入shopid,回傳對應同商店的items值(物品資料)
        orderby:根據什麼欄位排序
        sc:順序(asc)or逆序(desc)
        limitbase:從第幾個id開始
        limitoffset:要取幾個值
        比如要取id從2到7的值:就是limitbase=1,limitoffset=7*/
        shopitems[] vvv = new shopitems[100];
        string tables = "shop_items,items";
        try
        {
            Debug.Log("sqlapi: " + "tables,id:" + tables + "," + ids + "," + type);
            //SqlAccess sql = new SqlAccess();
            DataTable RT = sql.SelectWhere(tables, new string[] { "*" }, new string[] { "shop_items.shop_id", "items.id", "items.main_type" }, new string[] { "=", "=", "="}, new string[] { ids.ToString(), "shop_items.item_id", type.ToString() },
                 new string[] { orderby }, new string[] { sc }, new string[] { limitbase.ToString() }, new string[] { limitoffset.ToString() }).Tables[0];
            int i = 0;
            foreach (DataRow row in RT.Rows)
            {
                //foreach (DataColumn dc in RT.Columns) {
                //    if (typeof(int) == (row[dc].GetType())) {
                //        row[dc]=Convert.ToInt32(row[dc]);
                //    }
                //    else if(typeof(bool) == (row[dc].GetType())){
                //        row[dc]=Convert.ToBoolean(row[dc]);
                //    }
                //    else {
                //        row[dc] = row[dc].ToString();
                //    }
                //    Debug.Log(dc+":"+row[dc]+":"+row[dc].GetType());
                //}
                if (row != null)
                {
                    //vvv[i].id = Convert.ToInt32(row["id"]);
                    vvv[i].name = row["name"].ToString();
                    vvv[i].pic_url = row["pic_url"].ToString();
                    vvv[i].main_type = Convert.ToInt32(row["main_type"]);
                    vvv[i].sub_type = (float)Convert.ToDouble(row["sub_type"]);
                    vvv[i].description = row["description"].ToString();
                    vvv[i].enabled = Convert.ToBoolean(row["enabled"]);
                    vvv[i].model_name = row["model_name"].ToString();
                    vvv[i].model_linkurl = row["model_linkurl"].ToString();
                    vvv[i].standard_scale = row["standard_scale"].ToString();
                    vvv[i].panel_scale = Convert.ToDouble(row["panel_scale"]);
                    vvv[i].created_at = row["created_at1"].ToString();
                    vvv[i].updated_at = row["updated_at1"].ToString();

                    vvv[i].shop_id = Convert.ToInt32(row["shop_id"]);
                    vvv[i].item_id = Convert.ToInt32(row["item_id"]);
                    vvv[i].cost = Convert.ToInt32(row["cost"]);
                    vvv[i].enable = Convert.ToBoolean(row["enable"]);
                    vvv[i].click_times = Convert.ToInt32(row["click_times"]);
                    vvv[i].s_created_at = row["created_at"].ToString();
                    vvv[i].s_updated_at = row["updated_at"].ToString();
                    i++;
                }
                else
                {
                    Debug.Log("NAME NOT FOUND");
                    break;
                }
            }
            //Debug.Log("i=" + i); //counter
            //Debug.Log("col=" + RT.Columns.Count);
            //Debug.Log("row=" + RT.Rows.Count);
            //sql.Close();
            Array.Resize<shopitems>(ref vvv, i); //array(vvv)的大小設成counter(i)的大小
            return vvv;
        }
        catch (Exception e)
        {
            this.Error = e.Message;
        }
        return vvv;
    }

    /* ADD(insert) */
    public bool Add_shopcart(int userid, int itemid, int amount)
    {
        string tables = "shop_carts";
        try
        {
            Debug.Log("sqlapi: " + "tables,id:" + tables + ", " + userid + ", " + itemid);
            //SqlAccess sql = new SqlAccess();
            sql.InsertInto(tables, new string[] { "user_id", "item_id", "amount" },
                new string[] { userid.ToString(), itemid.ToString(), amount.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    public bool Add_userinvent(int userid, int itemid, int amount)
    {
        string tables = "user_inventories";
        try
        {
            Debug.Log("sqlapi: " + "tables,id:" + tables + ", " + userid + ", " + itemid);
            //SqlAccess sql = new SqlAccess();
            sql.InsertInto(tables, new string[] { "user_id", "item_id", "amount", "locked" },
                new string[] { userid.ToString(), itemid.ToString(), amount.ToString(), "1" });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    /* update */
    public bool Up_users(int ids, double money)
    {
        string tables = "users";
        try
        {
            Debug.Log("sqlapi: " + "tables,id:" + tables + ", " + ids);
            //SqlAccess sql = new SqlAccess();
            sql.UpdateInto("users", new string[] { "money" }, new string[] { money.ToString() }, new string[] { "id" }, new string[] { ids.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    public bool Up_userinvent(int userid, int itemid, int amount)
    {
        string tables = "user_inventories";
        try
        {
            Debug.Log("inventamount:"+amount);
            //SqlAccess sql = new SqlAccess();
            sql.UpdateInto(tables, new string[] { "amount" }, new string[] { amount.ToString() }, 
                new string[] { "user_id","item_id"}, new string[] { userid.ToString(), itemid.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    public bool Up_userinventlock(int userid, int itemid, bool isLock)
    {
        string tables = "user_inventories";
        try
        {
            //Debug.Log("inventamount:" + amount);
            //SqlAccess sql = new SqlAccess();
            sql.UpdateInto(tables, new string[] { "locked" }, new string[] { isLock.ToString() },
                new string[] { "user_id", "item_id" }, new string[] { userid.ToString(), itemid.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    public bool Up_shopcart(int userid, int itemid, int amount)
    {
        string tables = "shop_carts";
        try
        {
            Debug.Log("cartamount:" + amount);
            //SqlAccess sql = new SqlAccess();
            sql.UpdateInto(tables, new string[] { "amount" }, new string[] { amount.ToString() },
                new string[] { "user_id", "item_id" }, new string[] { userid.ToString(), itemid.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    public bool Up_item_sscale(int itemid, string s_scale)
    {
        string tables = "items";
        try
        {
            Debug.Log("standard_scale:" + s_scale);
            //SqlAccess sql = new SqlAccess();
            sql.UpdateInto(tables, new string[] { "standard_scale" }, new string[] { s_scale.ToString() },
                new string[] { "id" }, new string[] { itemid.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    /* delete */
    public bool Del_userinvent(int userid, int itemid)
    {
        string tables = "user_inventories";
        try
        {
            Debug.Log("inventid:" + itemid);
            //SqlAccess sql = new SqlAccess();
            sql.Delete(tables, new string[] { "user_id", "item_id" }, new string[] { userid.ToString(), itemid.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }

    public bool Del_shopcart(int userid, int itemid)
    {
        string tables = "shop_carts";
        try
        {
            Debug.Log("delete from cart:" + itemid);
            //SqlAccess sql = new SqlAccess();
            sql.Delete(tables, new string[] { "user_id", "item_id" }, new string[] { userid.ToString(), itemid.ToString() });
            //sql.Close();
            return true;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
        return false;
    }
}
