using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WorkshopDataModel 
{
    public string name;
    public TMP_Text count;
    public Button details_btn;
    public Image img;
    public Texture image;

    public string profession_type;
    public string crafter;
    public string durability;
    public string mat_need;
    public string function;
    public string rarity;
}
