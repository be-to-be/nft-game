using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MaterialDataModel
{
    public string name;
    public TMP_Text count;
    public Image material_img;
    public Texture image;
    public Texture image_multi;


    public Button detail_btn;
    public string type;
    public string end_product;
    public string profession;
}
