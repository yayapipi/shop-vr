using UnityEngine;
using System;
using System.Data;
using System.Collections;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
public class NewBehaviourScript : MonoBehaviour
{



    string Error = null;
    void Start()
    {
        try
        {

            //SqlAccess sql = new SqlAccess();

            /*sql.CreateTableAutoID("momo",new string[]{"id","name","qq","email","blog"}, new string[]{"int","text","text","text","text"});
            sql.CreateTable("momo",new string[]{"name","qq","email","blog"}, new string[]{"text","text","text","text"});
            sql.InsertInto("momo",new string[]{"name","qq","email","blog"},new string[]{"xuanyusong","289187120","xuanyusong@gmail.com","xuanyusong.com"});
            sql.InsertInto("momo",new string[]{"name","qq","email","blog"},new string[]{"ruoruo","34546546","ruoruo@gmail.com","xuanyusong.com"});*/



            //DataSet ds  = sql.SelectWhere("password_resets",new string[]{"email","token"},new string []{"id"},new string []{"="},new string []{"1"});
            //DataSet ds = sql.InsertInto("password_resets", new string[] {  "email", "token" }, new string[] { "xuanyusong@gmail.com", "xuanyusong.com" });
            sqlapi test = new sqlapi();

            /*user*/
            users vvc = test.getusers(1);
            Debug.Log("id:"+vvc.id+" name:"+vvc.name+"money:"+vvc.money+" modellink:"+vvc.user_model_link+" wordlink:"+vvc.world_asset_link);

            /*pics item*/
            //pics vvc = test.getpics(3);
            //pics[] vvcs = test.Ritem_pic(3);
            //foreach (pics vvc in vvcs)
            //{
            //    Debug.Log("id"+vvc.id+", item_id"+vvc.item_id+", pic_url"+vvc.pic_url);
            //}

            /*shop item*/
            //shopitems[] vvcs = test.Rshop_item(1, "id", "asc", 0, 100);
            //foreach (shopitems vvc in vvcs)
            //{
            //    Debug.Log("aa");
            //    Debug.Log("id=" + vvc.id + ", name=" + vvc.name + ", cost=" + vvc.cost);
            //}
            //Debug.Log("items'id=" + test.getname());
            //DataSet ds = sql.SelectWhere("items", new string[] { "*" }, new string[] { }, new string[] { }, new string[] { });
            //DataSet ds = sql.Asterisk("items");

            //if (ds != null)
            //{
            //    DataTable table = ds.Tables[0];

            //    foreach (DataRow row in table.Rows)
            //    {
            //        foreach (DataColumn column in table.Columns)
            //        {
            //            Debug.Log(row[column]);
            //        }
            //        //Debug.Log(row[0]);
            //    }
            //}
            //else
            //{
            //    Debug.Log("ds=null");
            //}



            /*sql.UpdateInto("momo",new string[]{"name","qq"},new string[]{"'ruoruo'","'11111111'"}, "email", "'xuanyusong@gmail.com'"  );

            sql.Delete("momo",new string[]{"id","email"}, new string[]{"1","'000@gmail.com'"}  );*/
            //sql.Close();
        }
        catch (Exception e)
        {
            Error = e.Message;
        }


    }

    // Update is called once per frame
    void OnGUI()
    {

        if (Error != null)
        {
            GUILayout.Label(Error);
        }

    }
}

