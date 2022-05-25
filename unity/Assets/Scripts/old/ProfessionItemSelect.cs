using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfessionItemSelect : MonoBehaviour
{
    // by l
    public GameObject EquipButton;
    public GameObject UnequipButton;
    public TMP_Text UseLeftText;

    public RawImage tool_img;
    public TMP_Text tool_name;
    public TMP_Text tool_function;
    public TMP_Text tool_durability;
    public GameObject textPanels;
    public GameObject parent;
    public Button show_btn;
    public string tool_id;
    public string toolname;
    public bool current_equipped;
    public string type;

    public string funtion_type;
}
