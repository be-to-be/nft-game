using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarehouseShow : BaseView
{
    [Space]
    [Header("MainPanel")]
    public GameObject MainMaterialPanel;
    public Transform MaterailGroup;
    public GameObject OneMaterialPrefab;
    [Space]
    [Header("MainPopup")]
    public GameObject OneMaterialPopup;
    public TMP_Text PopupTitle;
    public TMP_Text TopSectionTitle;
    public TMP_Text LeftSectionTitle;
    public RawImage LeftSectionImage;
    public TMP_Text LeftSectionInfo;
    public TMP_Text RightSectionTitle;
    public RawImage RightSctionImage;
    public TMP_Text RightSectionInfo;
    public GameObject MintButton;
    public GameObject AddButton;

    [Space]
    [Header("MintPopup")]
    public GameObject MintPopup;
    public TMP_Text MintPopupTitle;
    public TMP_Text MintPopupTopInfo;
    public TMP_Text MintPopupBottomInfo;
    public GameObject MintPopupYesButton;
    public GameObject NotMintPopup;
    public TMP_Text NotMintPopupTitle;
    public TMP_Text NotMintPopupTopInfo;
    public TMP_Text NotMintPopupBottomInfo;
    [Space]
    [Header("AddPopup")]
    public GameObject AddPopup;
    public TMP_Text AddPopupTitle;
    public TMP_Text AddPopupTopInfo;
    public TMP_Text AddPopupBottomInfo;
    public GameObject AddPopupYesButton;
    public GameObject NotAddPopup;
    public TMP_Text NotAddPopupTitle;
    public TMP_Text NotAddPopupTopInfo;
    public TMP_Text NotAddPopupBottomInfo;
    [Space]
    [Header("Variables")]
    public MaterialDataModel[] MaterialSchema;

    public GameObject material_panel;
    public GameObject child_panel;
    public GameObject craft_panel;
    public GameObject refine_panel;
    public GameObject select_prefab;
    public TMP_Text header;
    public TMP_Text mint_header;
    public TMP_Text add_header;
    public TMP_Text mint_text;
    public TMP_Text adD_text;
    public TMP_Text subheader_1;
    public TMP_Text subheader_2;
    public Image mint_img;
    public Image add_img;
    public Image refine_img;
    public Image done_panel_img;
    public Button add_btn;
    public AbbreviationsHelper helper;

    public GameObject PermissionPanel;
    public GameObject DonePanel;
    public GameObject DonePanel_Obj;
    public GameObject Loader;
    public GameObject MintText;
    public GameObject BurnText;
    public Button SellBtn;
    public Button RefineBtn;
    public Button MintBtn;
    public Button AddBtn;
    public Button BuyBtn;
    public Button YesBtn;

    public TMP_Text username;
    public TMP_Text citizens;
    public TMP_Text professions;
    public TMP_Text materials;
    public TMP_Text ninjas;
    public TMP_Text done_panel_text;
    protected override void Start()
    {
        base.Start();
        SetMaterials();
        MessageHandler.OnTransactionData += OnTransactionData;

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnTransactionData -= OnTransactionData;
    }

    public void SetMaterials()
    { 
        if (MaterailGroup.childCount >= 1)
        {
            foreach (Transform child in MaterailGroup)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        foreach (MaterialDataModel m in MaterialSchema)
        {
            var ins = Instantiate(OneMaterialPrefab);
            ins.transform.SetParent(MaterailGroup);
            ins.transform.localScale = new Vector3(1, 1, 1);
            var child = ins.gameObject.GetComponent<OneMaterialStatus>();
            child.CountText.text = "x0";
            child.image.texture = m.image;
            child.type = m.type;
            child.end_product = m.end_product;
            foreach (InventoryModel r in MessageHandler.userModel.inventory)
            {
                if (m.name == r.name){
                    child.CountText.text = "x" + r.count;
                }
            }
            child.DetailButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { DetailButtonClick(m); });
        }
    }
    public void DetailButtonClick(MaterialDataModel m)
    {
        Debug.Log("I am here");
        OneMaterialPopup.SetActive(true);
        string matName = helper.mat_abv[m.name];
        PopupTitle.text = matName;
        if (m.type == "raw" || m.type == "ore")
        {
            TopSectionTitle.text = "Refine raw materials into refined materials";
        }
        else
        {
            TopSectionTitle.text= "Use refined materials to craft items";
        }
        LeftSectionTitle.text = "Mint " + matName + " as NFT Pack of 10";
        LeftSectionInfo.text = "By using the 'Mint' button, you will be able to mint 10 " + matName +" into a '" + matName +" - 10x' NFT pack and 10 "
                                + matName  + " will be deducted from your account. Afterwards you will be able to sell that NFT on the market or trade with other players.";
        LeftSectionImage.texture = m.image_multi;
        RightSectionTitle.text = "Add " + matName;
        RightSectionInfo.text = "By using the 'Add' button, you will burn a '" + matName + "' - 10x' NFT pack and 10 '" + matName+"' will be added to your account.";
        RightSctionImage.texture = m.image_multi;
        MintButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { MintButtonClick(m.name, matName); });
        AddButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { AddButtonClick(m.name, matName); });

    }
    public void MintButtonClick(string keyName, string matName)
    {
        foreach(InventoryModel inv in MessageHandler.userModel.inventory)
        {
            if(inv.name == keyName)
            {
                if (Int64.Parse(inv.count) >= 10)
                {
                    MintPopup.SetActive(true);
                    MintPopupTitle.text = "afa";
                    MintPopupTopInfo.text ="rr";
                    MintPopupBottomInfo.text = "afa";
                    MintPopupYesButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { MintPopupYesButtonClick(keyName); });
                }
                else if (Int64.Parse(inv.count) < 10)
                {
                    NotMintPopup.SetActive(true);
                    NotMintPopupTitle.text = "afa";
                    NotMintPopupTopInfo.text ="rr";
                    NotMintPopupBottomInfo.text = "afa";
                }
                break;
            }
        }
    }
    public void MintPopupYesButtonClick(string keyName)
    {
        MessageHandler.Server_MintMat(keyName, "10");
    }
    public void AddButtonClick(string keyName, string matName)
    {
        foreach(InventoryModel inv in MessageHandler.userModel.inventory)
        {
            if(inv.name == keyName)
            {
                if (Int64.Parse(inv.count) >= 10)
                {
                    AddPopup.SetActive(true);
                    AddPopupTitle.text = "afa";
                    AddPopupTopInfo.text ="rr";
                    AddPopupBottomInfo.text = "afa";
                    AddPopupYesButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { AddPopupYesButtonClick(keyName); });
                }
                else if (Int64.Parse(inv.count) < 10)
                {
                    NotAddPopup.SetActive(true);
                    NotAddPopupTitle.text = "afa";
                    NotAddPopupTopInfo.text ="rr";
                    NotAddPopupBottomInfo.text = "afa";
                }
                break;
            }
        }
    }
    public void AddPopupYesButtonClick(string keyName)
    {
        MessageHandler.Server_BurnMat(keyName);
    }



    private void OnTransactionData()
    {
        if (MessageHandler.transactionModel.transactionid != "")
        {
            LoadingPanel.SetActive(false);
            DonePanel.SetActive(true);
            DonePanel_Obj.SetActive(true);
            string[] new_String = MessageHandler.transactionModel.transactionid.Split(' ');
            string action_type = new_String[0];
            string mat_name = new_String[1];
            foreach (MaterialDataModel m_data in MaterialSchema)
            {
                if (m_data.name == mat_name)
                {
                    done_panel_img.sprite = m_data.material_img.sprite;
                    break;
                }
            }
            if (action_type == "Mint")
            {
                done_panel_text.text = "The " + helper.mat_abv[mat_name] + " - 10x NFT was added to your wallet";
                MessageHandler.userModel.total_matCount = MessageHandler.transactionModel.citizens;
                materials.text = MessageHandler.userModel.total_matCount;

            }
            if (action_type == "Burn")
            {
                done_panel_text.text = "10 " + helper.mat_abv[mat_name] + " have been added to your account";
                MessageHandler.userModel.total_matCount = MessageHandler.transactionModel.citizens;
                materials.text = MessageHandler.userModel.total_matCount;
            }
            if(action_type == "None")
            {
                DonePanel.SetActive(false);
                SSTools.ShowMessage("Nothing to Burn", SSTools.Position.bottom, SSTools.Time.twoSecond);
            }

        }
    }

}
