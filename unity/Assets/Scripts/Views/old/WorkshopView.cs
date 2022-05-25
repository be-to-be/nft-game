using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorkshopView : BaseView
{
    private List<ItemDataModel> CHammer = new List<ItemDataModel>();
    private List<ItemDataModel> CSaw = new List<ItemDataModel>();
    private List<ItemDataModel> CSickle = new List<ItemDataModel>();
    private List<ItemDataModel> CPickAxe = new List<ItemDataModel>();
    private List<ItemDataModel> CAxe = new List<ItemDataModel>();
    private List<ItemDataModel> CHoe = new List<ItemDataModel>();
    private List<ItemDataModel> BCart = new List<ItemDataModel>();
    private List<ItemDataModel> BWheelbarrow = new List<ItemDataModel>();
    private List<ItemDataModel> BWagon = new List<ItemDataModel>();

    private List<ItemDataModel> THammer = new List<ItemDataModel>();
    private List<ItemDataModel> TSaw = new List<ItemDataModel>();
    private List<ItemDataModel> TSickle = new List<ItemDataModel>();
    private List<ItemDataModel> TPickAxe = new List<ItemDataModel>();
    private List<ItemDataModel> TAxe = new List<ItemDataModel>();
    private List<ItemDataModel> THoe = new List<ItemDataModel>();
    private List<ItemDataModel> OCart = new List<ItemDataModel>();
    private List<ItemDataModel> OWheelBarrow = new List<ItemDataModel>();
    private List<ItemDataModel> OWagon = new List<ItemDataModel>();

    private List<ItemDataModel> IHammer = new List<ItemDataModel>();
    private List<ItemDataModel> ISaw = new List<ItemDataModel>();
    private List<ItemDataModel> ISickle = new List<ItemDataModel>();
    private List<ItemDataModel> IPickAxe = new List<ItemDataModel>();
    private List<ItemDataModel> IAxe = new List<ItemDataModel>();
    private List<ItemDataModel> IHoe = new List<ItemDataModel>();
    private List<ItemDataModel> TCart = new List<ItemDataModel>();
    private List<ItemDataModel> TWheelBarrow = new List<ItemDataModel>();
    private List<ItemDataModel> TWagon = new List<ItemDataModel>();

    public WorkshopDataModel[] wdata;
    public GameObject workshop_prefab;
    public GameObject items_panel;
    public GameObject items_panel2;
    public GameObject no_asset_panel;
    public GameObject title_panel;
    public GameObject asset_display_panel;
    public GameObject item_select_prefab;
    public GameObject permission_panel;
    public GameObject donePanel;
    public TMP_Text rarity_text;
    public TMP_Text function_text;
    public TMP_Text durability_text;
    public TMP_Text used_by_text;
    public TMP_Text material_text;
    public TMP_Text crafted_by_text;
    public TMP_Text item_name_text;
    public TMP_Text description1_text;
    public TMP_Text craft_text;
    public TMP_Text infotext1_text;
    public TMP_Text permission_panel_text;
    public TMP_Text done_panel_text;
    public Image item_img;
    public RawImage done_panel_img;
    public Transform parent_object;
    public TMP_Text username;
    public TMP_Text citizens;
    public TMP_Text professions;
    public TMP_Text materials;
    public TMP_Text ninjas;
    public ImgObjectView[] image;
    public AbbreviationsHelper helper;
    public Button YesBtn;

    protected override void Start()
    {
        base.Start();
        SetListData(MessageHandler.userModel.items);
        SetUI();
        MessageHandler.OnCallBackData += OnCallBackData;
        MessageHandler.OnTransactionData += OnTransactionData;
        MessageHandler.OnItemData += OnItemData;
        MessageHandler.OnInventoryData += OnInventoryData;
        MessageHandler.OnProfessionData += OnProfessionData;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnCallBackData -= OnCallBackData;
        MessageHandler.OnTransactionData -= OnTransactionData;
        MessageHandler.OnItemData -= OnItemData;
        MessageHandler.OnInventoryData -= OnInventoryData;
        MessageHandler.OnProfessionData -= OnProfessionData;
    }

    public void SetUI()
    {
        username.text = MessageHandler.userModel.account;
        citizens.text = MessageHandler.userModel.citizens;
        professions.text = MessageHandler.userModel.professions.Length.ToString();
        materials.text = MessageHandler.userModel.items.Length.ToString();
        ninjas.text = MessageHandler.userModel.ninjas.Length.ToString();
        foreach (WorkshopDataModel w_data in wdata)
        {
            switch (w_data.name)
            {
                case "CHammer":
                    w_data.count.text = CHammer.Count.ToString();
                    if (CHammer.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Copper Hammer and Chisel"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(CHammer,w_data); });
                    }
                    break;
                case "CPickAxe":
                    w_data.count.text = CPickAxe.Count.ToString();
                    if (CPickAxe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Copper PickAxe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(CPickAxe,w_data); });
                    }
                    break;
                case "CSaw":
                    w_data.count.text = CSaw.Count.ToString();
                    if (CSaw.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Copper Saw"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(CSaw,w_data); });
                    }
                    break;
                case "CAxe":
                    w_data.count.text = CAxe.Count.ToString();
                    if (CAxe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Copper Axe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(CAxe,w_data); });
                    }
                    break;
                case "CSickle":
                    w_data.count.text = CSickle.Count.ToString();
                    if (CSickle.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Copper Sickle"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(CSickle,w_data); });
                    }
                    break;
                case "CHoe":
                    w_data.count.text = CHoe.Count.ToString();
                    if (CHoe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Copper Hoe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(CHoe,w_data); });
                    }
                    break;
                case "BWagon":
                    w_data.count.text = BWagon.Count.ToString();
                    if (BWagon.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Birch Wagon"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(BWagon,w_data); });
                    }
                    break;
                case "OWagon":
                    w_data.count.text = OWagon.Count.ToString();
                    if (OWagon.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Oak Wagon"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(OWagon,w_data); });
                    }
                    break;
                case "TWagon":
                    w_data.count.text = TWagon.Count.ToString();
                    if (TWagon.Count < 1)
                    {
                        Debug.Log("Twagon");
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Teak Wagon"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TWagon,w_data); });
                    }
                    break;
                case "IHoe":
                    w_data.count.text = IHoe.Count.ToString();
                    if (IHoe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Iron Hoe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(IHoe,w_data); });
                    }
                    break;
                case "THoe":
                    w_data.count.text = THoe.Count.ToString();
                    if (THoe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Tin Hoe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(THoe,w_data); });
                    }
                    break;
                case "ISickle":
                    w_data.count.text = ISickle.Count.ToString();
                     if (ISickle.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Iron Sickle"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(ISickle,w_data); });
                    }
                    break;
                case "TSickle":
                    w_data.count.text = TSickle.Count.ToString();
                    if (TSickle.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Tin Sickle"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TSickle,w_data); });
                    }
                    break;
                case "TWheelBarrow":
                    w_data.count.text = TWheelBarrow.Count.ToString();
                    if (TWheelBarrow.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Teak WheelBarrow"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TWheelBarrow,w_data); });
                    }
                    break;
                case "OWheelBarrow":
                    w_data.count.text = OWheelBarrow.Count.ToString();
                    if (OWheelBarrow.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Oak WheelBarrow"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(OWheelBarrow,w_data); });
                    }
                    break;
                case "BWheelbarrow":
                    w_data.count.text = BWheelbarrow.Count.ToString();
                    if (BWheelbarrow.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Birch WheelBarrow"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(BWheelbarrow,w_data); });
                    }
                    break;
                case "IAxe":
                    w_data.count.text = IAxe.Count.ToString();
                    if (IAxe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Iron Axe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(IAxe,w_data); });
                    }
                    break;
                case "TAxe":
                    w_data.count.text = TAxe.Count.ToString();
                    if (TAxe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Tin Axe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TAxe,w_data); });
                    }
                    break;
                case "ISaw":
                    w_data.count.text = ISaw.Count.ToString();
                    if (ISaw.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Iron Saw"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TSaw,w_data); });
                    }
                    break;
                case "TSaw":
                    w_data.count.text = TSaw.Count.ToString();
                    if (TSaw.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Tin Saw"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TSaw,w_data); });
                    }
                    break;
                case "TCart":
                    w_data.count.text = TCart.Count.ToString();
                    if (TCart.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Teak MiningCart"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TCart,w_data); });
                    }
                    break;
                case "OCart":
                    w_data.count.text =OCart.Count.ToString();
                    if (OCart.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Oak Mining Cart"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(OCart,w_data); });
                    }
                    break;
                case "BCart":
                    w_data.count.text =BCart.Count.ToString();
                    if (BCart.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Birch Mining Cart"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(BCart,w_data); });
                    }
                    break;
                case "IPickAxe":
                    w_data.count.text = IPickAxe.Count.ToString();
                    if (IPickAxe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Iron PickAxe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(IPickAxe,w_data); });
                    }
                    break;
                case "TPickAxe":
                    w_data.count.text = TPickAxe.Count.ToString();
                    if (TPickAxe.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Tin PickAxe"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(TPickAxe,w_data); });
                    }
                    break;
                case "IHammer":
                    w_data.count.text = IHammer.Count.ToString();
                    if (IHammer.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Iron Hammer"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(IHammer,w_data); });
                    }
                    break;
                case "THammer":
                    w_data.count.text = THammer.Count.ToString();
                    if (THammer.Count < 1)
                    {
                        UnityEngine.Color alpha = w_data.img.color;
                        alpha.a = 0.5f;
                        w_data.img.color = alpha;
                        w_data.details_btn.onClick.AddListener(delegate { Display_No_Asset(w_data, "Tin Hammer"); });
                    }
                    else
                    {
                        w_data.details_btn.onClick.AddListener(delegate { Display_Asset(THammer,w_data); });
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void SetListData(ItemDataModel[] items)
    {
        foreach(ItemDataModel idata in items)
        {
            switch (idata.name)
            {
                case "Copper Hammer and Chisel":
                    CHammer.Add(idata);
                    break;
                case "Copper Pickaxe":
                    CPickAxe.Add(idata);
                    break;
                case "Copper Saw":
                    CSaw.Add(idata);
                    break;
                case "Copper Axe":
                    CAxe.Add(idata);
                    break;
                case "Copper Sickle":
                    CSickle.Add(idata);
                    break;
                case "Copper Hoe":
                    CHoe.Add(idata);
                    break;
                case "Birch Wagon":
                    BWagon.Add(idata);
                    break;
                case "Oak Wagon":
                    OWagon.Add(idata);
                    break;
                case "Teak Wagon":
                    TWagon.Add(idata);
                    break;
                case "Iron Hoe":
                    IHoe.Add(idata);
                    break;
                case "Tin Hoe":
                    THoe.Add(idata);
                    break;
                case "Iron Sickle":
                    ISickle.Add(idata);
                    break;
                case "Tin Sickle":
                    TSickle.Add(idata);
                    break;
                case "Teak Wheelbarrow":
                    TWheelBarrow.Add(idata);
                    break;
                case "Oak Wheelbarrow":
                    OWheelBarrow.Add(idata);
                    break;
                case "Birch Wheelbarrow":
                    BWheelbarrow.Add(idata);
                    break;
                case "Iron Axe":
                    IAxe.Add(idata);
                    break;
                case "Tin Axe":
                    TAxe.Add(idata);
                    break;
                case "Iron Saw":
                    ISaw.Add(idata);
                    break;
                case "Tin Saw":
                    TSaw.Add(idata);
                    break;
                case "Teak Mining Cart":
                    TCart.Add(idata);
                    break;
                case "Oak Mining Cart":
                    OCart.Add(idata);
                    break;
                case "Birch Mining Cart":
                    BCart.Add(idata);
                    break;
                case "Iron Pickaxe":
                    IPickAxe.Add(idata);
                    break;
                case "Tin Pickaxe":
                    TPickAxe.Add(idata);
                    break;
                case "Iron Hammer and Chisel":
                    IHammer.Add(idata);
                    break;
                case "Tin Hammer and Chisel":
                    THammer.Add(idata);
                    break;
                default:
                    break;
            }
        }
    }

    public void Display_No_Asset(WorkshopDataModel wdata,string name)
    {
        foreach (Transform child in parent_object.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        items_panel.SetActive(false);
        items_panel2.SetActive(true);
        title_panel.SetActive(true);
        no_asset_panel.SetActive(true);
        item_img.sprite = wdata.img.sprite;
        infotext1_text.text = "The " + name + " is 1 of 3 different items that can be equipped by the " + wdata.profession_type + " profession to boost the gathering process";
        item_name_text.text = name;
        function_text.text = wdata.function;
        material_text.text = wdata.mat_need;
        rarity_text.text = wdata.rarity;
        durability_text.text = wdata.durability;
        used_by_text.text = wdata.profession_type;
        crafted_by_text.text = wdata.crafter;
        craft_text.text = "~ Unfortunately you don't have any " + name;
    }

    public void Display_Asset(List<ItemDataModel> idata,WorkshopDataModel wdata)
    {
        foreach (Transform child in parent_object.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        items_panel.SetActive(false);
        items_panel2.SetActive(true);
        title_panel.SetActive(true);
        asset_display_panel.SetActive(true);
        item_img.sprite = wdata.img.sprite;
        infotext1_text.text = "The " + idata[0].name + " is 1 of 3 different items that can be equipped by the " + wdata.profession_type + " profession to boost the gathering process";
        item_name_text.text = idata[0].name;
        function_text.text = wdata.function;
        material_text.text = wdata.mat_need;
        rarity_text.text = wdata.rarity;
        durability_text.text = wdata.durability;
        used_by_text.text = wdata.profession_type;
        crafted_by_text.text = wdata.crafter;

        for (int i = 0; i < idata.Count; i++)
        {
            var ins = Instantiate(workshop_prefab);
            ins.transform.SetParent(parent_object);
            ins.transform.localScale = new Vector3(1, 1, 1);
            var child = ins.gameObject.GetComponent<ItemCall>();
            child.asset_ids.text = idata[i].asset_id.ToString();
            child.asset_id = idata[i].asset_id;
            child.LoadingPanel = LoadingPanel;
            child.durability.text = "Durability : " + idata[i].uses_left + "/60";
            child.img.sprite = wdata.img.sprite;
            child.mat_name = idata[i].name;
            if (idata[i].equipped == "1")
            {
                child.unequip.SetActive(true);
                child.burn.gameObject.GetComponent<Button>().interactable = false;
                child.sell.gameObject.GetComponent<Button>().interactable = false;
                child.p_id = idata[i].profession;
                YesBtn.onClick.RemoveAllListeners();
                YesBtn.onClick.AddListener(delegate { child.BurnBtn(); });
            }
            else if (idata[i].equipped == "0")
            {
                child.equip.SetActive(true);
                string item_name = idata[i].name;
                child.equip.gameObject.GetComponent<Button>().onClick.AddListener(delegate { EquipItem(item_name); });
                if(idata[i].uses_left == "0")
                {
                    child.equip.gameObject.GetComponent<Button>().interactable = false;
                    child.sell.gameObject.GetComponent<Button>().interactable = false;
                }
            }
        }

    }

    public void returnToItems()
    {
        items_panel.SetActive(true);
        items_panel2.SetActive(false);
        title_panel.SetActive(false);
        asset_display_panel.SetActive(false);
        no_asset_panel.SetActive(false);
        SetUI();
    }

    public void OnItemData()
    {
        CHammer.Clear();
        CSaw.Clear();
        CSickle.Clear();
        CPickAxe.Clear();
        CAxe.Clear();
        CHoe.Clear();
        BCart.Clear();
        BWheelbarrow.Clear();
        THammer.Clear();
        TSaw.Clear();
        TSickle.Clear();
        TPickAxe.Clear();
        TAxe.Clear();
        THoe.Clear();
        OCart.Clear();
        OWheelBarrow.Clear();
        OWagon.Clear();
        IHammer.Clear();
        ISaw.Clear();
        ISickle.Clear();
        IPickAxe.Clear();
        IAxe.Clear();
        IHoe.Clear();
        TCart.Clear();
        TWheelBarrow.Clear();
        TWagon.Clear();
        SetListData(MessageHandler.userModel.items);
    }

    public void OnTransactionData()
    {
        if (MessageHandler.transactionModel.transactionid != "")
        {
            LoadingPanel.SetActive(false);
            WorkshopDataModel item_data = new WorkshopDataModel();
            string item_name = "";
            switch (MessageHandler.transactionModel.transactionid)
            {
                case "Copper Hammer and Chisel":
                    item_name = "CHammer";
                    break;
                case "Copper Pickaxe":
                    item_name = "CPickAxe";
                    break;
                case "Copper Saw":
                    item_name = "CSaw";
                    break;
                case "Copper Axe":
                    item_name = "CAxe";
                    break;
                case "Copper Sickle":
                    item_name = "CSickle";
                    break;
                case "Copper Hoe":
                    item_name = "CHoe";
                    break;
                case "Birch Wagon":
                    item_name = "BWagon";
                    break;
                case "Oak Wagon":
                    item_name = "OWagon";
                    break;
                case "Teak Wagon":
                    item_name = "TWagon";
                    break;
                case "Iron Hoe":
                    item_name = "IHoe";
                    break;
                case "Tin Hoe":
                    item_name = "THoe";
                    break;
                case "Iron Sickle":
                    item_name = "ISickle";
                    break;
                case "Tin Sickle":
                    item_name = "TSickle";
                    break;
                case "Teak Wheelbarrow":
                    item_name = "TWheelBarrow";
                    break;
                case "Oak Wheelbarrow":
                    item_name = "OWheelBarrow";
                    break;
                case "Birch Wheelbarrow":
                    item_name = "BWheelbarrow";
                    break;
                case "Iron Axe":
                    item_name = "IAxe";
                    break;
                case "Tin Axe":
                    item_name = "TAxe";
                    break;
                case "Iron Saw":
                    item_name = "ISaw";
                    break;
                case "Tin Saw":
                    item_name = "TSaw";
                    break;
                case "Teak Mining Cart":
                    item_name = "TCart";
                    break;
                case "Oak Mining Cart":
                    item_name = "OCart";
                    break;
                case "Birch Mining Cart":
                    item_name = "BCart";
                    break;
                case "Iron Pickaxe":
                    item_name = "IPickAxe";
                    break;
                case "Tin Pickaxe":
                    item_name = "TPickAxe";
                    break;
                case "Iron Hammer and Chisel":
                    item_name = "IHammer";
                    break;
                case "Tin Hammer and Chisel":
                    item_name = "THammer";
                    break;
                default:
                    break;
            }
            foreach (WorkshopDataModel w_data in wdata)
            {
                if (w_data.name == item_name)
                {
                    item_data = w_data;
                    break;
                }
            }
            switch (item_name)
            {
                case "CHammer":
                    if (CHammer.Count > 0)
                        Display_Asset(CHammer, item_data);
                    else
                        Display_No_Asset(item_data, "Copper Hammer and Chisel");
                    break;
                case "CPickAxe":
                    if (CPickAxe.Count > 0)
                        Display_Asset(CPickAxe, item_data);
                    else
                        Display_No_Asset(item_data, "Copper PickAxe");
                    break;
                case "CSaw":
                    if (CSaw.Count > 0)
                        Display_Asset(CSaw, item_data);
                    else
                        Display_No_Asset(item_data, "Copper Saw");
                    break;
                case "CAxe":
                    if (CAxe.Count > 0)
                        Display_Asset(CAxe, item_data);
                    else
                        Display_No_Asset(item_data, "Copper Axe");
                    break;
                case "CSickle":
                    if (CSickle.Count > 0)
                        Display_Asset(CSickle, item_data);
                    else
                        Display_No_Asset(item_data, "Copper Sickle");
                    break;
                case "CHoe":
                    if (CHoe.Count > 0)
                        Display_Asset(CHoe, item_data);
                    else
                        Display_No_Asset(item_data, "Copper Hoe");
                    break;
                case "BWagon":
                    if (BWagon.Count > 0)
                        Display_Asset(BWagon, item_data);
                    else
                        Display_No_Asset(item_data, "Birch Wagon");
                    break;
                case "OWagon":
                    if (OWagon.Count > 0)
                        Display_Asset(OWagon, item_data);
                    else
                        Display_No_Asset(item_data, "Oak Wagon");
                    break;
                case "TWagon":
                    if (TWagon.Count > 0)
                        Display_Asset(TWagon, item_data);
                    else
                        Display_No_Asset(item_data, "Teak Wagon");
                    break;
                case "IHoe":
                    if (IHoe.Count > 0)
                        Display_Asset(IHoe, item_data);
                    else
                        Display_No_Asset(item_data, "Iron Hoe");
                    break;
                case "THoe":
                    if (THoe.Count > 0)
                        Display_Asset(THoe, item_data);
                    else
                        Display_No_Asset(item_data, "Tin Hoe");
                    break;
                case "ISickle":
                    if (ISickle.Count > 0)
                        Display_Asset(ISickle, item_data);
                    else
                        Display_No_Asset(item_data, "Iron Sickle");
                    break;
                case "TSickle":
                    if (TSickle.Count > 0)
                        Display_Asset(TSickle, item_data);
                    else
                        Display_No_Asset(item_data, "Tin Sickle");
                    break;
                case "TWheelBarrow":
                    if (TWheelBarrow.Count > 0)
                        Display_Asset(TWheelBarrow, item_data);
                    else
                        Display_No_Asset(item_data, "Teak WheelBarrow");
                    break;
                case "OWheelBarrow":
                    if (OWheelBarrow.Count > 0)
                        Display_Asset(OWheelBarrow, item_data);
                    else
                        Display_No_Asset(item_data, "Oak WheelBarrow");
                    break;
                case "BWheelbarrow":
                    if (BWheelbarrow.Count > 0)
                        Display_Asset(BWheelbarrow, item_data);
                    else
                        Display_No_Asset(item_data, "Birch WheelBarrow");
                    break;
                case "IAxe":
                    if (IAxe.Count > 0)
                        Display_Asset(IAxe, item_data);
                    else
                        Display_No_Asset(item_data, "Iron Axe");
                    break;
                case "TAxe":
                    if (TAxe.Count > 0)
                        Display_Asset(TAxe, item_data);
                    else
                        Display_No_Asset(item_data, "Tin Axe");
                    break;
                case "ISaw":
                    if (ISaw.Count > 0)
                        Display_Asset(TSaw, item_data);
                    else
                        Display_No_Asset(item_data, "Iron Saw");
                    break;
                case "TSaw":
                    if (TSaw.Count > 0)
                        Display_Asset(TSaw, item_data);
                    else
                        Display_No_Asset(item_data, "Tin Saw");
                    break;
                case "TCart":
                    if (TCart.Count > 0)
                        Display_Asset(TCart, item_data);
                    else
                        Display_No_Asset(item_data, "Teak MiningCart");
                    break;
                case "OCart":
                    if (OCart.Count > 0)
                        Display_Asset(OCart, item_data);
                    else
                        Display_No_Asset(item_data, "Oak Mining Cart");
                    break;
                case "BCart":
                    if (BCart.Count > 0)
                        Display_Asset(BCart, item_data);
                    else
                        Display_No_Asset(item_data, "Birch Mining Cart");
                    break;
                case "IPickAxe":
                    if (IPickAxe.Count > 0)
                        Display_Asset(IPickAxe, item_data);
                    else
                        Display_No_Asset(item_data, "Iron PickAxe");
                    break;
                case "TPickAxe":
                    if (TPickAxe.Count > 0)
                        Display_Asset(TPickAxe, item_data);
                    else
                        Display_No_Asset(item_data, "Tin PickAxe");
                    break;
                case "IHammer":
                    if (IHammer.Count > 0)
                        Display_Asset(IHammer, item_data);
                    else
                        Display_No_Asset(item_data, "Iron Hammer");
                    break;
                case "THammer":
                    if (THammer.Count > 0)
                        Display_Asset(THammer, item_data);
                    else
                        Display_No_Asset(item_data, "Tin Hammer");
                    break;
                default:
                    break;
            }
        }
    }

    public void OnCallBackData(CallBackDataModel[] callback)
    {
        CallBackDataModel callBack = callback[0];
        switch (callBack.status)
        {
            case ("De-Equiped Successfully"):
                SSTools.ShowMessage("Item De-Equiped Successful !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            default:
                break;
        }
    }

    public void EquipItem(string item_name)
    {
        var ins = Instantiate(item_select_prefab);
        var child = ins.gameObject.GetComponent<ItemSelectedModel>();
        child.item_selected = item_name;
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("ProfessionScene");
    }

    public void Show_BurnPanel(string item_name)
    {
        permission_panel.SetActive(true);
        permission_panel_text.text = "Do you really want to burn your " + item_name + "? If yes, you will have a some chance to get back 1 random material that was used for crafting it";
    }

    private void OnInventoryData(InventoryModel[] inventoryData)
    {
        InventoryModel inventory = inventoryData[0];
        LoadingPanel.SetActive(false);
        permission_panel.SetActive(false);
        donePanel.SetActive(true);
        if (inventory.name == "null")
        {
            done_panel_text.text = "No material could be extracted";
            done_panel_img.gameObject.SetActive(false);
        }
        else {
            done_panel_img.gameObject.SetActive(true);
            string item_name = helper.mat_abv[inventory.name];
            foreach(ImgObjectView img in image)
            {
                if(img.name == item_name)
                {
                    done_panel_img.texture = img.img;
                }
            }
            done_panel_text.text = inventory.count + " " + item_name + " was extracted!";
        }

    }

    public void NoBtn()
    {
        permission_panel.SetActive(false);
        donePanel.SetActive(false);
    }

    public void OnProfessionData(ProfessionDataModel[] pr) { }
}
