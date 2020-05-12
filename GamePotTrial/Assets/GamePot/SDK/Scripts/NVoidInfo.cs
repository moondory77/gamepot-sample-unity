using UnityEngine;
using System.Collections;
using Realtime.LITJson;


public class NVoidInfo
{
    public NVoidInfo()
    {
        headerTitle = "";
        descHTML = "";
        listHeaderTitle = "";
        footerTitle = "";
    }

    public string headerTitle { get; set; }
    public string descHTML { get; set; }
    public string listHeaderTitle { get; set; }
    public string footerTitle { get; set; }

    public string ToJson()
    {
        JsonData data = new JsonData();
        data["headerTitle"] = headerTitle;
        data["descHTML"] = descHTML;
        data["listHeaderTitle"] = listHeaderTitle;
        data["footerTitle"] = footerTitle;

        return data.ToJson();
    }


}