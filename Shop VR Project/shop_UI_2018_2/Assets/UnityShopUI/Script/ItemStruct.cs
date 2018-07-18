public struct ItemStruct{
    private int id;
    private string name;
    private int main_type;
    private int sub_type;
    private string description;
    private string model_linkurl;
    private string pic_linkurl;
    private float cost;

    public ItemStruct(int id, string name, int main_type, int sub_type, string description, string model_linkurl, string pic_linkurl, float cost)
    {
        this.id = id;
        this.name = name;
        this.main_type = main_type;
        this.sub_type = sub_type;
        this.description = description;
        this.model_linkurl = model_linkurl;
        this.pic_linkurl = pic_linkurl;
        this.cost = cost;
    }
    /*
    public ItemStruct(ItemStruct data)
    {
        this.id = id;
        this.name = name;
        this.main_type = main_type;
        this.sub_type = sub_type;
        this.description = description;
        this.model_linkurl = model_linkurl;
        this.pic_linkurl = pic_linkurl;
        this.cost = cost;
    }
    */

    public int Get_id()
    {
        return this.id;
    }

    public string Get_name()
    {
        return this.name;
    }

    public int Get_main_type()
    {
        return this.main_type;
    }

    public int Get_sub_type()
    {
        return this.sub_type;
    }

    public string Get_description()
    {
        return this.description;
    }

    public string Get_model_linkurl()
    {
        return this.model_linkurl;
    }

    public string Get_pic_linkurl()
    {
        return this.pic_linkurl;
    }

    public float Get_cost()
    {
        return this.cost;
    }
}
