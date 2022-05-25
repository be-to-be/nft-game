using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProfessionsView : BaseView
{
    [Space]
    [Header("Text Mesh Pro GUI")]
    public TMP_Text username;
    public TMP_Text citizens;
    public TMP_Text professions;
    public TMP_Text materials;
    public TMP_Text ninjas;
    public TMP_Text miner_count;
    public TMP_Text farmer_count;
    public TMP_Text engineer_count;
    public TMP_Text lumberjack_count;
    public TMP_Text tailor_count;
    public TMP_Text blacksmith_count;
    public TMP_Text carpenter_count;
    public TMP_Text profession_text;
    public TMP_Text details_text;
    public TMP_Text types_text;
    public TMP_Text citizens_text;
    public TMP_Text uses_text;
    public TMP_Text books_text;
    public TMP_Text name_text;
    public TMP_Text found_text;
    public TMP_Text refineProduct1_name;
    public TMP_Text refineProduct2_name;
    public TMP_Text text_permission;
    public TMP_Text done_panel_text;
    [Space]
    [Header("GameObjects")]
    public GameObject Text_Panel;
    public GameObject ProfessionTitle_Panel;
    public GameObject Profession_Panel;
    public GameObject Profession_Prefab;
    public GameObject Profession_ItemSelect_Prefab;
    public GameObject FoundPanel;
    public GameObject ItemPanel;
    public GameObject item_object;
    public GameObject items_textpanel;
    public GameObject craft_panel;
    public GameObject BlendingPanel;
    public GameObject refineProductPanel;
    public GameObject refineProductPrefab;
    public GameObject refinePanel;
    public GameObject SettlementParentPanel;
    public GameObject SettlementChildPanel;
    public GameObject UnregisteredSettlementChildPanel;
    public GameObject SettlementTextPanel;
    public GameObject SettlementBuyBtn;
    public GameObject SettlementDeregButton;
    public GameObject RegisteredSettlementChildPanel;
    public GameObject SettlementPrefab;
    public GameObject NoForest_Text;
    public GameObject PermissionPanel;
    public GameObject DonePanel;
    [Space]
    [Header("Raw Images")]
    public RawImage Found_Mat_Img;
    public RawImage refineProduct1;
    public RawImage refineProduct2;
    public RawImage BlendingPanel_img;
    public RawImage done_panel_img;
    [Space]
    [Header("Transform")]
    public Transform Profession_Parent_Obj;
    public Transform mainPanel;
    public Transform item_inventory_parent;
    public Transform currently_equipped_parent;
    public Transform refineParent;
    public Transform settlementObj;
    public Transform unregisteredSettlementObj;

    [Space]
    [Header("Buttons")]
    public Button refineBtn;
    public Button items_craftBtn;
    public Button crafts_btn;
    public Button upgrade_btn;
    public Button yesbtn;
    [Space]
    [Header("Data Models and Scripts")]
    public List<BlendingModel> BlendingData = new List<BlendingModel>();
    public ImgObjectView[] images;
    public AbbreviationsHelper helper;
    public RefineDataModel[] refineData;

    private List<ProfessionDataModel> Engineer = new List<ProfessionDataModel>();
    private List<ProfessionDataModel> Miners = new List<ProfessionDataModel>();
    private List<ProfessionDataModel> Farmers = new List<ProfessionDataModel>();
    private List<ProfessionDataModel> Tailor = new List<ProfessionDataModel>();
    private List<ProfessionDataModel> Carpenter = new List<ProfessionDataModel>();
    private List<ProfessionDataModel> LumberJack = new List<ProfessionDataModel>();
    private List<ProfessionDataModel> Blacksmith = new List<ProfessionDataModel>();
    private List<SettlementsModel> Forest = new List<SettlementsModel>();
    private GameObject profession_itemSelect = null;

    protected override void Start()
    {
        base.Start();
        SetModels(MessageHandler.userModel.professions);
        SetSettlementsModel(MessageHandler.userModel.settlements);
        SetUIElements();
        MessageHandler.OnProfessionData += OnProfessionData;
        MessageHandler.OnCallBackData += OnCallBackData;
        MessageHandler.OnSettlementData += OnSettlementData;
        MessageHandler.OnTransactionData += OnTransactionData;
        MessageHandler.OnItemData += OnItemData;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnProfessionData -= OnProfessionData;
        MessageHandler.OnCallBackData -= OnCallBackData;
        MessageHandler.OnSettlementData -= OnSettlementData;
        MessageHandler.OnTransactionData -= OnTransactionData;
        MessageHandler.OnItemData -= OnItemData;
    }

    private void SetModels(ProfessionDataModel[] professions)
    {
        foreach (ProfessionDataModel profess in professions)
        {
            if (profess.name == "Engineer")
            {
                Engineer.Add(profess);
            }
            else if(profess.name == "Tailor")
            {
                Tailor.Add(profess);
            }
            else if (profess.name == "Miner")
            {
                Miners.Add(profess);
            }
            else if (profess.name == "Blacksmith")
            {
                Blacksmith.Add(profess);
            }
            else if (profess.name == "Lumberjack")
            {
                LumberJack.Add(profess);
            }
            else if (profess.name == "Farmer")
            {
                Farmers.Add(profess);
            }
            else if (profess.name == "Carpenter")
            {
                Carpenter.Add(profess);
            }
        }
    }

    private void SetSettlementsModel(SettlementsModel[] settlements)
    {
        Forest.Clear();
        foreach (SettlementsModel data in settlements)
        {
            switch (data.name)
            {
                case ("Forest"):
                    Forest.Add(data);
                    break;
                default:
                    break;
            }
        }
    }
    private void SetUIElements()
    {
        if (MessageHandler.userModel.account != null)
        {
            username.text = MessageHandler.userModel.account;
            citizens.text = MessageHandler.userModel.citizens;
            professions.text = MessageHandler.userModel.professions.Length.ToString();
            materials.text = MessageHandler.userModel.total_matCount;
            ninjas.text = MessageHandler.userModel.ninjas.Length.ToString();
            miner_count.text = Miners.Count.ToString();
            farmer_count.text = Farmers.Count.ToString();
            engineer_count.text = Engineer.Count.ToString();
            tailor_count.text = Tailor.Count.ToString();
            lumberjack_count.text = LumberJack.Count.ToString();
            carpenter_count.text = Carpenter.Count.ToString();
            blacksmith_count.text = Blacksmith.Count.ToString();
            var obj = GameObject.Find("Selected_Item_GB(Clone)");
            if (obj != null)
            {
                helper.add_items();
                string item_name = obj.gameObject.GetComponent<ItemSelectedModel>().item_selected;
                ProfessionNftBtn(helper.equip_items_abv[item_name]);
                Destroy(obj); 
            }
        }
    }
    public void ProfessionNftBtn(string name)
    {
        Debug.Log(name);
        if(Text_Panel.activeInHierarchy)Text_Panel.SetActive(false);
        if(!ProfessionTitle_Panel.activeInHierarchy)ProfessionTitle_Panel.SetActive(true);
        switch (name)
        {
            case "Miner":
                if (Miners.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Miners, "Gatherer");
                    upgrade_btn.gameObject.SetActive(true);
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Miner");
                }
                break;
            case "Farmer":
                if (Farmers.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Farmers, "Gatherer");
                    upgrade_btn.gameObject.SetActive(true);
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Farmer");
                }
                break;
            case "Engineer":
                if (Engineer.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Engineer, "Refiner and Crafter");
                    if(upgrade_btn.gameObject.activeInHierarchy) upgrade_btn.gameObject.SetActive(false);
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData,"Engineer");
                }
                break;
            case "Carpenter":
                if (Carpenter.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Carpenter, "Refiner and Crafter");
                    if (upgrade_btn.gameObject.activeInHierarchy) upgrade_btn.gameObject.SetActive(false);
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData,"Carpenter");
                }
                break;
            case "Tailor":
                if (Tailor.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Tailor, "Refiner and Crafter");
                    if (upgrade_btn.gameObject.activeInHierarchy) upgrade_btn.gameObject.SetActive(false);
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData,"Tailor");
                }
                break;
            case "Blacksmith":
                if (Blacksmith.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Blacksmith, "Refiner and Crafter");
                    if (upgrade_btn.gameObject.activeInHierarchy) upgrade_btn.gameObject.SetActive(false);
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData,"Blacksmith");
                }
                break;
            case "Lumberjack":
                if (LumberJack.Count > 0)
                {
                    Debug.Log("In");
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(LumberJack, "Gatherer");
                    upgrade_btn.gameObject.SetActive(true);
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData,"Lumberjack");
                }
                break;
            default:
                break;
        }

    }

    public void SetBlendingData(List<BlendingModel> blendingModel,string name)
    {
        if (ItemPanel.activeInHierarchy) ItemPanel.SetActive(false);
        var obj = GameObject.Find("ProfessionAsset(Clone)");
        if (obj != null)
        {
            Destroy(obj);
        }
        string maxCount = "";
        foreach (MaxNftDataModel nftData in MessageHandler.userModel.nft_count)
        {
            if (nftData.name == name)
            {
                maxCount = nftData.count;
                break;
            }
        }
        if(!string.IsNullOrEmpty(maxCount))
            profession_text.text = name + "   " + "0" + "/" + maxCount;
        else 
            profession_text.text = name + "   " + "0" + "/" + "10";
        foreach (BlendingModel bmodel in blendingModel)
        {
            if(bmodel.profession == name)
            {
                int temp = 0;
                for (int j = 0; j < images.Length; j++)
                {
                    if (images[j].name == name)
                    {
                        temp = j;
                        break;
                    }
                }
                BlendingPanel_img.texture = images[temp].img;
                name_text.text = name;
                details_text.text = "Every couple of hours you will be able to refine raw materials like copper ore (common), tin ore (uncommon) and iron ore (rare) and craft items.You will then be able to either trade, sell or refine these raw materials.";
                uses_text.text = bmodel.uses;
                types_text.text = bmodel.types;
                citizens_text.text = bmodel.citizens;
                books_text.text = bmodel.books;
                break;
            }
        }
    }

    public void SetProfessions(List<ProfessionDataModel> professionModel,string type)
    {
        if (ItemPanel.activeInHierarchy) ItemPanel.SetActive(false);
        var obj = GameObject.Find("ProfessionAsset(Clone)");
        if (obj != null)
        {
            Destroy(obj);
        }
        string name = professionModel[0].name;
        string maxCount = "";

        foreach (MaxNftDataModel nftData in MessageHandler.userModel.nft_count)
        {
            if (nftData.name == name)
            {
                maxCount = nftData.count;
                break;
            }
        }

        if (!string.IsNullOrEmpty(maxCount)) 
            profession_text.text = name + "   " + professionModel.Count.ToString() + "/" + maxCount; 
        else 
            profession_text.text = name + "   " + professionModel.Count.ToString() + "/" + "10";
        
        int temp = 0;
        for (int j = 0; j < images.Length; j++)
        {
            if (images[j].name == name)
            {
                temp = j;
                break;
            }
        }

        if (Profession_Parent_Obj.childCount >= 1)
        {
            foreach (Transform child in Profession_Parent_Obj)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < professionModel.Count; i++)
        {
            var ins = Instantiate(Profession_Prefab);
            ins.transform.SetParent(Profession_Parent_Obj);
            ins.transform.localScale = new Vector3(1, 1, 1);
            var child = ins.gameObject.GetComponent<ProfessionCall>();
            child.asset_ids = professionModel[i].asset_id;
            child.name_nft.text = professionModel[i].name;
            child.type = professionModel[i].name;
            child.LoadingPanel = this.LoadingPanel;
            child.img.texture = images[temp].img;


            if (professionModel[i].reg == "0")
            {
                child.UnRegistered.SetActive(true);
                child.Sellbtn.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(child.BtnParent.GetComponent<RectTransform>().sizeDelta.x, child.Sellbtn.gameObject.GetComponent<RectTransform>().sizeDelta.y);
            }
            else if (professionModel[i].reg == "1")
            {
                child.Registered.SetActive(true);
                if (type == "Gatherer")
                {
                    child.WorkBtn.SetActive(true);
                    child.ItemBtn.SetActive(true);
                    child.item_count.text = professionModel[i].items.Length.ToString();
                    child.uses_left.text = professionModel[i].uses_left.ToString();
                    child.gatherer = true;
                    string professionName = professionModel[i].name;
                    string[] items = professionModel[i].items;
                    string p_id = professionModel[i].asset_id;
                    child.ItemBtn.gameObject.GetComponent<Button>().onClick.AddListener(delegate { showItems(items, professionName, p_id); });
                }

                else if (type == "Refiner and Crafter")
                {
                    child.gatherer = false;
                    CraftPanelCall craftScript = craft_panel.GetComponent<CraftPanelCall>();
                    craftScript.EngineerType.SetActive(false);
                    craftScript.BlacksmithType.SetActive(false);
                    craftScript.grade_obj_parent.SetActive(false);
                    craftScript.ing_obj_parent.SetActive(false);

                    if (professionModel[i].name == "Engineer")
                    {
                        child.UnRegister_Btn.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(child.Registered.gameObject.GetComponent<RectTransform>().sizeDelta.x, child.Registered.gameObject.GetComponent<RectTransform>().sizeDelta.y);
                        child.BurnBtn.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(child.Registered.gameObject.GetComponent<RectTransform>().sizeDelta.x, child.Registered.gameObject.GetComponent<RectTransform>().sizeDelta.y);
                        child.CraftBtn.SetActive(true);
                        child.craft_text.text = professionModel[i].uses_left.ToString();
                        string p_id = professionModel[i].asset_id;
                        child.CraftBtn.GetComponent<Button>().onClick.AddListener(delegate { OnCraftBtnClick(p_id); });
                        craftScript.profession_name = professionModel[i].name;
                        craftScript.EngineerType.SetActive(true);
                    }
                    else if (professionModel[i].name == "Tailor" || professionModel[i].name == "Carpenter")
                    {
                        child.RefineBtn.SetActive(true);
                        child.refine_text.text = professionModel[i].uses_left.ToString();
                        child.CraftBtn.SetActive(false);
                        child.Sellbtn.GetComponent<RectTransform>().sizeDelta = new Vector2(child.Registered.gameObject.GetComponent<RectTransform>().sizeDelta.x, child.Registered.gameObject.GetComponent<RectTransform>().sizeDelta.y);
                    }
                    else
                    {
                        child.RefineBtn.SetActive(true);
                        child.CraftBtn.SetActive(true);
                        child.refine_text.text = professionModel[i].uses_left.ToString();
                        child.craft_text.text = professionModel[i].uses_left.ToString();
                        string p_id = professionModel[i].asset_id;
                        child.CraftBtn.GetComponent<Button>().onClick.AddListener(delegate { OnCraftBtnClick(p_id); });
                        craftScript.profession_id = professionModel[i].asset_id;
                        craftScript.profession_name = professionModel[i].name;
                        craftScript.BlacksmithType.SetActive(true);
                    }
                    string profession_name = professionModel[i].name;
                    string asset_id = professionModel[i].asset_id;
                    child.RefineBtn.gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnRefineBtnClick(profession_name,asset_id); });
                    if (int.Parse(professionModel[i].uses_left) == 0)
                    {
                        child.WorkBtn.GetComponent<Button>().interactable = false;
                        child.RefineBtn.GetComponent<Button>().interactable = false;
                        child.CraftBtn.GetComponent<Button>().interactable = false;
                        child.UnRegister_Btn.gameObject.SetActive(false);
                        child.BurnBtn.gameObject.SetActive(true);
                        child.BurnBtn.onClick.RemoveAllListeners();
                        string p_name = professionModel[i].name;
                        child.BurnBtn.onClick.AddListener(delegate { Show_BurnPanel(p_name); });
                        yesbtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.AddListener(delegate { child.BurnBtn_Call(); });
                    }
                }

                if (professionModel[i].status != "Idle" || professionModel[i].last_material_search != "1970-01-01T00:00:00")
                {
                    if(professionModel[i].status == "Mining")
                        child.StartTimer(professionModel[i].last_material_search,100);
                    else if(professionModel[i].status == "holdup")
                    {
                        child.WorkBtn.gameObject.GetComponent<Button>().interactable = false;
                        SSTools.ShowMessage("One or more Asset ID is under holdup status", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    }
                    else
                    {
                        string[] newstring = professionModel[i].status.Split('%');
                        switch (newstring[0])
                        {
                            case ("Refining"):
                                child.matName = newstring[1];
                                child.craft = false;
                                child.StartTimer(professionModel[i].last_material_search,100);
                                break;
                            case ("Crafting"):
                                child.matName = newstring[1];
                                child.craft = true;
                                foreach(Config_CraftComboModel craftData in MessageHandler.userModel.craft_combos)
                                {
                                    if(newstring[1] == craftData.template_id)
                                    {
                                        child.StartTimer(professionModel[i].last_material_search, (int)Int64.Parse(craftData.delay));
                                        Debug.Log((int)Int64.Parse(craftData.delay));
                                        break;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    public void Show_BurnPanel(string p_name)
    {
        PermissionPanel.SetActive(true);
        text_permission.text = "Do you really want to burn your " + p_name + "? If yes, you will have a some chance to get back 1 citizen";
    }

    public void showItems(string[] item_ids,string profession_name,string profession_id)
    {

        Profession_Panel.SetActive(false);
        profession_text.text = profession_name + " #" + profession_id;
        var ins = Instantiate(Profession_ItemSelect_Prefab);
        ins.transform.SetParent(mainPanel);
        ins.transform.localScale = new Vector3(1, 1, 1);
        profession_itemSelect = ins;
        var child = ins.gameObject.GetComponent<ItemSelectBtnHelper>();
        child.profession_name = profession_name;
        child.professiom_asset_id = profession_id;
        child.backBtn.onClick.AddListener(delegate { items_backBtn(ins, child.profession_name); });
        items_craftBtn.onClick.AddListener(delegate { items_backBtn(ins, child.profession_name); });
        ProfessionItemSelect[] child_objs = child.Button;
        foreach(ProfessionItemSelect select in child_objs)
        {
            select.show_btn.onClick.AddListener(delegate { Equip_items_panel(select,profession_id); });
        }
        foreach (ImgObjectView data in images)
        {
            if (profession_name == data.name)
            {
                child.profession_img.texture = data.img;
                break;
            }
        }
        
        for (int i = 0; i < item_ids.Length; i++)
        {
            foreach (ItemDataModel items in MessageHandler.userModel.items)
            {
                if(items.asset_id == item_ids[i])
                {
                    foreach(ImgObjectView data in images)
                    {
                        if(items.name == data.name)
                        {
                            foreach(ProfessionItemSelect select in child_objs)
                            {
                                if(select.funtion_type == items.function_name)
                                {

                                    select.tool_id = items.asset_id;
                                    select.tool_img.texture = data.img;
                                    select.textPanels.SetActive(true);
                                    select.tool_name.text = "Name : " + items.name;
                                    select.tool_function.text = "Function : " + items.function_value + "% " + items.function_name;
                                    select.tool_durability.text = "Durability : " + items.uses_left + "/60";
                                    select.current_equipped = true;
                                    select.toolname = items.name;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }

    public void Equip_items_panel(ProfessionItemSelect gb,string profession_id)
    {
        Debug.Log(profession_id);
        ItemPanel.SetActive(true);
        ItemsPanelView itemPanel = ItemPanel.gameObject.GetComponent<ItemsPanelView>();
        Debug.Log(itemPanel.p_id);
        itemPanel.p_id = profession_id;
        Debug.Log(itemPanel.p_id);
        itemPanel.loadingPanel = LoadingPanel;
        string profession_name = gb.parent.gameObject.GetComponent<ItemSelectBtnHelper>().profession_name;
        string[] item_names = helper.profession_equip_items[profession_name];
        if (item_inventory_parent.childCount >= 1)
        {
            foreach (Transform child in item_inventory_parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        foreach (string i_name in item_names)
        {
            foreach (ItemDataModel items in MessageHandler.userModel.items)
            {
                if (items.equipped == "0" && i_name == items.name && items.function_name == gb.funtion_type)
                {
                    var ins = Instantiate(item_object);
                    ins.transform.SetParent(item_inventory_parent);
                    ins.transform.localScale = new Vector3(1, 1, 1);
                    var child = ins.gameObject.GetComponent<ItemSelectCall>();
                    child.item_name.text = "Name : " + items.name;
                    child.functionality.text = "Function : " + items.function_value + "% " + items.function_name;
                    child.durability.text = "Durability : " + items.uses_left + "/60";
                    child.item_image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { change_equip_id(items.asset_id,itemPanel); });
                    foreach (ImgObjectView data in images)
                    {
                        if (items.name == data.name)
                        {
                            child.item_image.texture = data.img; 
                            break;
                        }
                    }
                }
            }
        }

        if (item_inventory_parent.childCount > 0)
        {
            if(items_textpanel.activeInHierarchy)items_textpanel.SetActive(false);
            itemPanel.Equip_btn.interactable = true;
        }
        else
        {
            items_textpanel.SetActive(true);
            items_craftBtn.onClick.AddListener(delegate { Okbtn(); });
        }

        if (currently_equipped_parent.childCount >= 1)
        {
            foreach (Transform child in currently_equipped_parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        if (gb.current_equipped == true)
        {
            var ins = Instantiate(item_object);
            ins.transform.SetParent(currently_equipped_parent);
            ins.transform.localScale = new Vector3(1, 1, 1);
            var child = ins.gameObject.GetComponent<ItemSelectCall>();
            child.item_name.text = gb.tool_name.text;
            child.functionality.text = gb.tool_function.text;
            child.durability.text = gb.tool_durability.text;
            child.item_image.texture = gb.tool_img.texture;
            itemPanel.unequip_id = gb.tool_id;
            Debug.Log("From Profession View - " + gb.tool_id);
            Debug.Log("From Profession View - " + itemPanel.unequip_id);
            itemPanel.unequip_item_name = gb.toolname;
            itemPanel.Unequip_btn.interactable = true;
        }
    }

    private void change_equip_id(string id,ItemsPanelView child_obj)
    {
        child_obj.equip_id = id;
    }

    public void items_backBtn(GameObject child,string profession_name)
    {
        Destroy(child.gameObject);
        Profession_Panel.SetActive(true);
        ProfessionNftBtn(profession_name);
    }

    public void OnProfessionData(ProfessionDataModel[] profession)
    {
        Engineer.Clear();
        Miners.Clear();
        LumberJack.Clear();
        Tailor.Clear();
        Farmers.Clear();
        Carpenter.Clear();
        Blacksmith.Clear();
        SetModels(profession);
    }

    public void OnCallBackData(CallBackDataModel[] callback)
    {
        CallBackDataModel callBack = callback[0];
        switch (callBack.name)
        {
            case "Miner":
                if (Miners.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Miners, "Gatherer");
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Miner");
                }
                break;
            case "Farmer":
                if (Farmers.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Farmers, "Gatherer");
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Farmer");
                }
                break;
            case "Engineer":
                if (Engineer.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Engineer, "Refiner and Crafter");
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Engineer");
                }
                break;
            case "Carpenter":
                if (Carpenter.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Carpenter, "Refiner and Crafter");
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Carpenter");
                }
                break;
            case "Tailor":
                if (Tailor.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Tailor, "Refiner and Crafter");
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Tailor");
                }
                break;
            case "Blacksmith":
                if (Blacksmith.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(Blacksmith, "Refiner and Crafter");
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Blacksmith");
                }
                break;
            case "Lumberjack":
                if (LumberJack.Count > 0)
                {
                    if (!Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(true);
                    if (BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(false);
                    SetProfessions(LumberJack, "Gatherer");
                }
                else
                {
                    if (Profession_Panel.activeInHierarchy) Profession_Panel.SetActive(false);
                    if (!BlendingPanel.activeInHierarchy) BlendingPanel.SetActive(true);
                    SetBlendingData(BlendingData, "Lumberjack");
                }
                break;
            case ("Forest"):
                ShowSettlements(Forest);
                break;
            default:
                break;
        }

        LoadingPanel.SetActive(false);
        switch (callBack.status)
        {
            case ("Registered Successfully"):
                SSTools.ShowMessage("NFT Registration Successful !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            case ("De-Registered Successfully"):
                SSTools.ShowMessage("NFT De-Registration Successful !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            case ("Item Equipped"):
                SSTools.ShowMessage("Item Equiped Successfully !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            case ("De-Equiped"):
                SSTools.ShowMessage("Item De-Equiped Successfully !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            case ("Refining Started"):
                SSTools.ShowMessage("Refining Started !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                Okbtn();
                break;
            case ("Crafting Started"):
                SSTools.ShowMessage("Crafting Started !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                Okbtn();
                break;
            case ("RNG Failed !"):
                SSTools.ShowMessage("RNG Failed !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                Okbtn();
                break;
            case ("Burnt Successfully"):
                SSTools.ShowMessage("NFT Burn Successful !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            default:
                break;
        }

        if (!string.IsNullOrEmpty(callBack.matFound) && callBack.matFound == "true" && !string.IsNullOrEmpty(callBack.matRefined) && callBack.matRefined == "false")
        {
            if (!string.IsNullOrEmpty(callBack.matName) || !string.IsNullOrEmpty(callBack.matCount))
            {
                MessageHandler.userModel.total_matCount = callBack.totalMatCount;
                materials.text = MessageHandler.userModel.total_matCount;
                if (!FoundPanel.activeInHierarchy) FoundPanel.SetActive(true);
                foreach(ImgObjectView data in images)
                {
                    if(data.name == callBack.matName)
                    {
                        Found_Mat_Img.texture = data.img;
                        found_text.text = "Found " + callBack.matCount + " " + helper.mat_abv[callBack.matName];
                        break;
                    }
                }
            }
        }
        else if (!string.IsNullOrEmpty(callBack.matFound) && !string.IsNullOrEmpty(callBack.matRefined) && callBack.matFound == "false" && callBack.matRefined == "true")
        {
            if (!string.IsNullOrEmpty(callBack.matName))
            {
                Debug.Log(callBack.matName);
                MessageHandler.userModel.total_matCount = callBack.totalMatCount;
                materials.text = MessageHandler.userModel.total_matCount;
                if (!FoundPanel.activeInHierarchy) FoundPanel.SetActive(true);
                string refined_mat = helper.mat_abv[callBack.matName];
                Debug.Log(refined_mat);
                foreach (RefineDataModel refine_mat in refineData)
                {
                    if (refine_mat.name == refined_mat)
                    {
                        Found_Mat_Img.texture = refine_mat.refined_product_img;
                        found_text.text = "Successfully Refined " + refine_mat.name + " " + "to " + refine_mat.refined_product;
                        break;
                    }
                }
            }
        }

        else if(!string.IsNullOrEmpty(callBack.matFound) && !string.IsNullOrEmpty(callBack.matRefined) && callBack.matFound == "false" && callBack.matRefined == "false" && callBack.equipped == "1")
        {

            showItems(callBack.items_ids,callBack.name,callBack.asset_id);
        }

        else if (!string.IsNullOrEmpty(callBack.matFound) && !string.IsNullOrEmpty(callBack.matRefined) && callBack.matFound == "false" && callBack.matRefined == "false" && callBack.equipped == "0")
        {

            showItems(callBack.items_ids, callBack.name, callBack.asset_id);
        }

        else if (!string.IsNullOrEmpty(callBack.matFound) && !string.IsNullOrEmpty(callBack.matRefined) && !string.IsNullOrEmpty(callBack.matCrafted) && callBack.matFound == "false" && callBack.matRefined == "false" && callBack.matCrafted == "true")
        {

            if (!string.IsNullOrEmpty(callBack.matName))
            {
                Debug.Log(callBack.matName);
                MessageHandler.userModel.total_matCount = callBack.totalMatCount;
                materials.text = MessageHandler.userModel.total_matCount;
                if (!FoundPanel.activeInHierarchy) FoundPanel.SetActive(true);
                foreach(ImgObjectView img in images)
                {
                    if(img.name == callBack.matName)
                    {
                        Found_Mat_Img.texture = img.img;
                        found_text.text = "Successfully Crafted  " + callBack.matName;
                        break;
                    }
                }
            }
        }

    }

    public void ProfessionItemSelect_Back(string type)
    {
        switch (type)
        {
            case "Miner":
                SetProfessions(Miners, "Gatherer");
                break;
            case "Farmer":
                SetProfessions(Farmers, "Gatherer");
               
                break;
            case "Engineer":
                SetProfessions(Engineer, "Refiner and Crafter");
                
                break;
            case "Carpenter":
                SetProfessions(Carpenter, "Refiner and Crafter");
                
                break;
            case "Tailor":
                SetProfessions(Tailor, "Refiner and Crafter");
                
                break;
            case "Blacksmith":
                SetProfessions(Blacksmith, "Refiner and Crafter");
               
                break;
            case "Lumberjack":
                SetProfessions(LumberJack, "Gatherer");
               
                break;
            default:
                break;
        }
    }

    public void OnCraftBtnClick(string profession_id)
    {
        craft_panel.SetActive(true);
        Debug.Log("Craft P Id - " + profession_id);
        craft_panel.GetComponent<CraftPanelCall>().profession_id = profession_id;
        Debug.Log("Craft P Id - " + craft_panel.GetComponent<CraftPanelCall>().profession_id);
    }

    public void OnRefineBtnClick(string profession, string profession_id)
    {
        refinePanel.SetActive(true);
        if (refineParent.childCount >= 1)
        {
            foreach (Transform child in refineParent)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (RefineDataModel refine_mat in refineData)
        {
            if(refine_mat.profession == profession)
            {
                Debug.Log("Profession Found " + profession);
                var ins = Instantiate(refineProductPrefab);
                ins.transform.SetParent(refineParent);
                ins.transform.localScale = new Vector3(1, 1, 1);
                var child = ins.gameObject.GetComponent<RefineMatCall>();
                child.mat_name.text = "Name : " + refine_mat.name;
                child.nft_img.texture = refine_mat.img;
                child.rarity.text = "Rarity : " + refine_mat.rarity;
                string qty = "0";
                string material_short_name = helper.mat_abv_rev[refine_mat.name];
                if (MessageHandler.userModel.inventory.Length == 0)
                {
                    child.qty.text = "Qty : 0";
                }
                else
                {
                    bool found = false;
                    foreach (InventoryModel data in MessageHandler.userModel.inventory)
                    {
                        Debug.Log("In QTY For Loop");
                        if (data.name == material_short_name)
                        {
                            Debug.Log("Found Qty");
                            child.qty.text = "Qty :" + data.count;
                            qty = data.count;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        child.qty.text = "Qty : 0";
                }
                if(qty != "0")
                    child.showRefines.onClick.AddListener(delegate { Refine_MatOnClick(refine_mat.name,profession_id); });
                else
                {
                    child.showRefines.onClick.AddListener(delegate { SSTools.ShowMessage("Not Enough Qty to Refine", SSTools.Position.bottom, SSTools.Time.twoSecond); });
                }
                Debug.Log("Button Set !");

            }
        }
    }

    public void Refine_MatOnClick(string mat_name,string profession_id)
    {
        refineProductPanel.SetActive(true);
        string profession_name = "";
        foreach (RefineDataModel refine_mat in refineData)
        {
            if(refine_mat.name == mat_name)
            {
                Debug.Log(mat_name);
                refineProduct1_name.text = mat_name;
                refineProduct2_name.text = refine_mat.refined_product;
                refineProduct1.texture = refine_mat.img;
                refineProduct2.texture = refine_mat.refined_product_img;
                profession_name = refine_mat.profession;
                break;
            }
        }
        refineBtn.gameObject.GetComponent<Button>().interactable = true;
        refineBtn.onClick.RemoveAllListeners();
        foreach(DelayDataModel data in MessageHandler.userModel.config.rawmat_refined)
        {
            if(data.key == helper.mat_abv_rev[mat_name])
            {
                refineBtn.onClick.AddListener(delegate { RefineTrxCall(data.value, profession_id,profession_name); });
                break;
            }
        }
    }

    public void RefineTrxCall(string mat_name,string profession_id,string profession_name)
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_RefineMat(profession_id,mat_name, profession_name);
    }

    public void ShowCraft_Rarity(CraftDataModel arr)
    {
        string mat_name = arr.craft_name; string type = arr.profession_name;
        CraftPanelCall craftScript = craft_panel.GetComponent<CraftPanelCall>();
        if (!craftScript.grade_obj_parent.gameObject.activeInHierarchy) craftScript.grade_obj_parent.gameObject.SetActive(true);
       
        foreach(ItemSelectCall gb in craftScript.grade_obj)
        {
            foreach(Config_CraftComboModel data_model in MessageHandler.userModel.craft_combos)
            {
                if(data_model.item_name == mat_name && gb.name == data_model.rarity)
                {
                    string craft_name = "";
                    if (type == "Engineer")
                    {
                        switch (data_model.rarity)
                        {
                            case ("Common"):
                                craft_name = "Birch " + mat_name;
                                break;
                            case ("Uncommon"):
                                craft_name = "Oak " + mat_name;
                                break;
                            case ("Rare"):
                                craft_name = "Teak " + mat_name;
                                break;
                        }
                    }
                    else
                    {
                        switch (data_model.rarity)
                        {
                            case ("Common"):
                                craft_name = "Copper " + mat_name;
                                break;
                            case ("Uncommon"):
                                craft_name = "Tin " + mat_name;
                                break;
                            case ("Rare"):
                                craft_name = "Iron " + mat_name;
                                break;
                        }
                    }
                    gb.item_name.text = "Name : " + craft_name;
                    gb.functionality.text = "Rarity : " + data_model.rarity;
                    TimeSpan t = TimeSpan.FromSeconds(Double.Parse(data_model.delay));
                    gb.durability.text = "Time to craft : " + t.Hours + " Hours";
                    foreach(ImgObjectView img in images)
                    {
                        if(img.name == craft_name)
                        {
                            gb.item_image.texture = img.img;
                            break;
                        }
                    }
                    gb.item_image.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    gb.item_image.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Show_CraftIngredients(mat_name,craft_name,data_model); });
                    break;
                }
            }
        }
    }

    public void Show_CraftIngredients(string mat_name,string craft_name,Config_CraftComboModel c_data)
    {
        CraftPanelCall craftScript = craft_panel.GetComponent<CraftPanelCall>();
        if (!craftScript.ing_obj_parent.gameObject.activeInHierarchy) craftScript.ing_obj_parent.gameObject.SetActive(true);
        DelayDataModel[] ingredients_craft = new DelayDataModel[3];
        foreach (Config_CraftComboModel data_model in MessageHandler.userModel.craft_combos)
        {
            if (data_model.item_name == mat_name && data_model.rarity == c_data.rarity)
            {
                ingredients_craft = data_model.ingredients;
                break;
            }
        }

        bool canCraft = true;
        for (int i = 0; i < 3; i++)
        {
            foreach (ImgObjectView img in images)
            {
                if (img.name == ingredients_craft[i].key)
                {
                    craftScript.craft_img[i].texture = img.img;
                    craftScript.ing_name[i].text = helper.mat_abv[ingredients_craft[i].key];
                    if (MessageHandler.userModel.inventory.Length == 0)
                    {
                        craftScript.ing_qty[i].text = 0 + "/" + ingredients_craft[i].value;
                        craftScript.ing_qty[i].color = new Color32(212, 27, 29, 255);
                        break;
                    }
                    else
                    {
                        bool found = false;
                        foreach (InventoryModel inv_data in MessageHandler.userModel.inventory)
                        {
                            if (inv_data.name == ingredients_craft[i].key)
                            {
                                craftScript.ing_qty[i].text = inv_data.count + "/" + ingredients_craft[i].value;
                                if (Int64.Parse(inv_data.count) >= Int64.Parse(ingredients_craft[i].value))
                                {
                                    craftScript.ing_qty[i].color = new Color32(94, 173, 53, 255);
                                }
                                else if (Int64.Parse(inv_data.count) < Int64.Parse(ingredients_craft[i].value))
                                { 
                                    craftScript.ing_qty[i].color = new Color32(212, 27, 29, 255);
                                    canCraft = false;
                                }
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            craftScript.ing_qty[i].text = 0 + "/" + ingredients_craft[i].value;
                            craftScript.ing_qty[i].color = new Color32(212, 27, 29, 255);
                        }
                        break;
                    }
                }
            }
        }
        foreach (ImgObjectView img in images)
        { 
            if(craft_name == img.name)
            {
                bool profession_found = false;
                string[] professions_arr = { "Miner", "Lumberjack", "Farmer" };
                for(int m=0; m < professions_arr.Length; m++)
                {
                    string[] items = helper.profession_equip_items[professions_arr[m]];
                    foreach(string item_name_data in items)
                    {
                        if(item_name_data == craft_name)
                        {
                            craftScript.product_equipped_By.text = "Equippable By : " + professions_arr[m];
                            profession_found = true;
                            break;
                        }

                    }
                    if (profession_found) break;

                }
                craftScript.end_product.texture = img.img;
                craftScript.product_name.text = "Name : " + craft_name;
                craftScript.product_rarity.text = "Rarity : " + c_data.rarity;
                if (c_data.rarity == "Common")
                    craftScript.product_function.text = "Function : 5% " + c_data.type;
                else if (c_data.rarity == "Uncommon")
                    craftScript.product_function.text = "Function : 10% " + c_data.type;
                else
                    craftScript.product_function.text = "Function : 15% " + c_data.type;
                craftScript.product_durability.text = "Durability : 60/60";
                break;
            }
        }
        if (canCraft)
        {
            crafts_btn.interactable = true;
            crafts_btn.onClick.RemoveAllListeners();
            Debug.Log("Can Craft - " + canCraft);
            Debug.Log("Craft P Id - " + craftScript.profession_id);
            crafts_btn.onClick.AddListener(delegate { CraftTrx(craftScript.profession_id,c_data.template_id,craftScript.profession_name); });
        }
            
    }

    public void CraftTrx(string profession_id,string template,string profession_name)
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_CraftMat(profession_id, template, profession_name);
    }

    public void Okbtn()
    {
        if (FoundPanel.activeInHierarchy) FoundPanel.SetActive(false);
        if (refinePanel.activeInHierarchy)
        {
            refinePanel.SetActive(false);
            refineProductPanel.SetActive(false);
            refineBtn.onClick.RemoveAllListeners();
        }
        if (ItemPanel.activeInHierarchy) ItemPanel.SetActive(false);
        if (items_textpanel.activeInHierarchy) items_textpanel.SetActive(false);
        if (craft_panel.activeInHierarchy) craft_panel.SetActive(false);
        if (crafts_btn.interactable) crafts_btn.interactable = false;
        if (DonePanel.activeInHierarchy) DonePanel.SetActive(false);
        if (PermissionPanel.activeInHierarchy) PermissionPanel.SetActive(false);
    }

    public void CrossBtn()
    {
        SettlementParentPanel.SetActive(false);
        SettlementChildPanel.SetActive(false);
        UnregisteredSettlementChildPanel.SetActive(false);
        SettlementTextPanel.SetActive(false);
        SettlementBuyBtn.SetActive(false);
        SettlementDeregButton.SetActive(false);
        RegisteredSettlementChildPanel.SetActive(false);
        NoForest_Text.SetActive(false);
    }

    public void BuyUpgrades()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=upgrades&template_id=282662");
    }

    public void BuyBtn()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=professions");
    }

    public void ShowSettlements(List<SettlementsModel> forest)
    {
        NoForest_Text.SetActive(false);
        if (unregisteredSettlementObj.childCount >= 1)
        {
            foreach (Transform child in unregisteredSettlementObj)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        if (settlementObj.childCount >= 1)
        {
            foreach (Transform child in settlementObj)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        if (forest.Count >= 1)
        {
            SettlementChildPanel.SetActive(true);
            UnregisteredSettlementChildPanel.SetActive(true);
            RegisteredSettlementChildPanel.SetActive(true);
            SettlementDeregButton.SetActive(true);
            foreach (SettlementsModel data in forest)
            {
                var ins = Instantiate(SettlementPrefab);
                var child = ins.gameObject.GetComponent<SettlementCall>();
                child.nft_name.text = "Name : " + data.name;
                child.nftImg.loadimg("https://ipfs.wecan.dev/ipfs/" + data.img);
                child.asset_id = data.asset_id;
                if (data.reg == "0")
                {
                    ins.transform.SetParent(unregisteredSettlementObj);
                    ins.transform.localScale = new Vector3(1, 1, 1);
                    child.Register.SetActive(true);
                    child.Register.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Register_Settlement(data.asset_id); });
                }

                else if (data.reg == "1")
                {
                    ins.transform.SetParent(settlementObj);
                    ins.transform.localScale = new Vector3(1, 1, 1);
                    child.DeRegister.SetActive(true);
                    child.DeRegister.gameObject.GetComponent<Button>().onClick.AddListener(delegate { DeRegister_Settlement(data.asset_id); });
                }

            }
            if (settlementObj.childCount == 0)
                SettlementDeregButton.gameObject.GetComponent<Button>().interactable = false;
            if (unregisteredSettlementObj.childCount == 0)
                NoForest_Text.SetActive(true);
        }
        else if (forest.Count == 0)
        {
            SettlementTextPanel.SetActive(true);
            SettlementBuyBtn.SetActive(true);
        }
    }

    public void UpgradeBtn()
    {
        SettlementParentPanel.SetActive(true);
        ShowSettlements(Forest);

    }

    public void Register_Settlement(string asset_id)
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_TransferAsset(asset_id, "regupgrade","Forest");
    }

    public void DeRegister_Settlement(string asset_id)
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_WithdrawAsset(asset_id,"Forest");
    }

    public void OnSettlementData(SettlementsModel[] settlements)
    {
        SetSettlementsModel(settlements);
    }

    public void OnTransactionData()
    {

        if (MessageHandler.transactionModel.transactionid != "")
        {
            string[] new_string = MessageHandler.transactionModel.transactionid.Split('%');
            if(new_string.Length > 1)
            {
                LoadingPanel.SetActive(false);
                PermissionPanel.SetActive(false);
                DonePanel.SetActive(true);
                MessageHandler.userModel.citizens = MessageHandler.transactionModel.citizens;
                citizens_text.text = MessageHandler.userModel.citizens;
                string citizens_found = new_string[1];
                if (Int64.Parse(citizens_found) == 0)
                {
                    done_panel_img.gameObject.SetActive(false);
                    done_panel_text.text = "No Citizen Survived !";
                }
                else
                {
                    done_panel_img.gameObject.SetActive(true);
                    done_panel_text.text = "1 Citizen Survived !";
                }
            }
        }
    }

    public void OnItemData() { }
}
