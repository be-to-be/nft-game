using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaterialView : BaseView
{
    public MaterialDataModel[] mdata;
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
        SetUI();
        MessageHandler.OnTransactionData += OnTransactionData;

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnTransactionData -= OnTransactionData;
    }

    public void SetUI()
    {
        username.text = MessageHandler.userModel.account;
        citizens.text = MessageHandler.userModel.citizens;
        professions.text = MessageHandler.userModel.professions.Length.ToString();
        materials.text = MessageHandler.userModel.total_matCount;
        ninjas.text = MessageHandler.userModel.ninjas.Length.ToString();
        if (MessageHandler.userModel.inventory.Length == 0)
        {
            foreach (MaterialDataModel m_data in mdata)
            {
                UnityEngine.Color alpha = m_data.material_img.color;
                alpha.a = 0.5f;
                m_data.material_img.color = alpha;
            }
        }

        else
        {
            foreach (InventoryModel idata in MessageHandler.userModel.inventory)
            {
                foreach (MaterialDataModel m_data in mdata)
                {
                    if (idata.name == m_data.name)
                    {
                        m_data.count.text = idata.count;
                        m_data.detail_btn.onClick.AddListener(delegate { ShowPanel(m_data); });
                        if (idata.count == "0")
                        {
                            UnityEngine.Color alpha = m_data.material_img.color;
                            alpha.a = 0.5f;
                            m_data.material_img.color = alpha;
                        }
                    }
                }
            }
        }
    }

    public void ShowPanel(MaterialDataModel material)
    {
        if (material_panel.activeInHierarchy) material_panel.SetActive(false);
        child_panel.SetActive(true);
        MintText.gameObject.GetComponent<TMP_Text>().text = "Do you really want to mint 10 " + material.name + " into an NFT ?";
        BurnText.gameObject.GetComponent<TMP_Text>().text = "Do you really want to burn a " + material.name + " - 10x NFT and add 10 " + material.name + " to your account?";
        if (material.type == "raw" || material.type == "ore")
        {
            craft_panel.SetActive(false);
            refine_panel.SetActive(true);
            refine_img.sprite = material.material_img.sprite;
            mint_img.sprite = material.material_img.sprite;
            add_img.sprite = material.material_img.sprite;
            string matName = helper.mat_abv[material.name];
            header.text = "Refine " + matName + " to " + material.end_product;
            subheader_1.text = "If you have at least 3 " + matName + " and the " + material.profession + " , you will be able to refine them into an " + material.end_product;
            subheader_2.text = "You can then use the refined " + material.end_product + " together with other refined materials to craft items if you have the appropiate profession";
            mint_header.text = "Mint " + matName + " as NFT Pack";
            add_header.text = "Add " + matName;
            mint_text.text = "By using the 'Mint' button you will be able to mint 10 " + matName + " into a " + matName + " - 10x NFT which you then will be able to sell on the market or trade with other people";
            adD_text.text = "By using the 'Add' button you will burn a " + matName + " - 10x NFT and 10 " + matName + " will be added to your account";
            string pack_name = matName + " - 10x";
            MintText.gameObject.GetComponent<TMP_Text>().text = "Do you really want to mint 10 " + matName + " into an NFT ?";
            BurnText.gameObject.GetComponent<TMP_Text>().text = "Do you really want to burn a " + matName + " - 10x NFT and add 10 " + matName + " to your account?";
            RefineBtn.onClick.RemoveAllListeners();
            RefineBtn.onClick.AddListener(delegate { RefineButton(material.profession); });
            foreach (AssetModel asset in MessageHandler.userModel.assets)
            {
                if (asset.name == pack_name)
                    add_btn.interactable = true;
            }
        }
        else
        {
            string matName = helper.mat_abv[material.name];
            refine_panel.SetActive(false);
            craft_panel.SetActive(true);
            refine_img.sprite = material.material_img.sprite;
            mint_img.sprite = material.material_img.sprite;
            add_img.sprite = material.material_img.sprite;
            header.text = "Craft Items";
            mint_header.text = "Mint " + matName + " as NFT Pack";
            add_header.text = "Add " + matName;
            mint_text.text = "By using the 'Mint' button you will be able to mint 10 " + matName + " into a " + matName + " - 10x NFT which you then will be able to sell on the market or trade with other people";
            adD_text.text = "By using the 'Add' button you will burn a " + matName + " - 10x NFT and 10 " + matName + " will be added to your account";
            MintText.gameObject.GetComponent<TMP_Text>().text = "Do you really want to mint 10 " + matName + " into an NFT ?";
            BurnText.gameObject.GetComponent<TMP_Text>().text = "Do you really want to burn a " + matName + " - 10x NFT and add 10 " + matName + " to your account?";
            string pack_name = matName + " - 10x";
            foreach (AssetModel asset in MessageHandler.userModel.assets)
            {
                if (asset.name == pack_name)
                    add_btn.interactable = true;
            }
        }

        MintBtn.onClick.AddListener(delegate { MintButton(material.name); });
        AddBtn.onClick.AddListener(delegate { AddButton(material.name); });
    }

    public void CraftButton()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("ProfessionScene");
    }

    public void RefineButton(string professionName)
    {
        var ins = Instantiate(select_prefab);
        var child = ins.gameObject.GetComponent<SelectedRefineModel>();
        child.ProfessionName = professionName;
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("ProfessionScene");
    }

    public void MintButton(string mat_name)
    {
        BurnText.SetActive(false);
        foreach(InventoryModel inv in MessageHandler.userModel.inventory)
        {
            if(inv.name == mat_name)
            {
                if (Int64.Parse(inv.count) >= 10)
                {
                    PermissionPanel.SetActive(true);
                    MintText.SetActive(true);
                    YesBtn.onClick.RemoveAllListeners();
                    YesBtn.onClick.AddListener(delegate { YesButton_Mint(mat_name); });
                }
                else if (Int64.Parse(inv.count) < 10)
                {
                    SSTools.ShowMessage("In-Sufficient Balance", SSTools.Position.bottom, SSTools.Time.threeSecond);
                }
                break;
            }
        }
    }

    public void SellButton()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=items");
    }

    public void AddButton(string mat_name)
    {
        PermissionPanel.SetActive(true);
        MintText.SetActive(false);
        BurnText.SetActive(true);
        YesBtn.onClick.RemoveAllListeners();
        YesBtn.onClick.AddListener(delegate { YesButton_Burn(mat_name); });
    }

    public void BuyButton()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=items");
    }

    public void YesButton_Mint(string mat_name)
    {
        Debug.Log(mat_name + "Mint");
        PermissionPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        MessageHandler.Server_MintMat(mat_name, "10");
        
    }

    public void YesButton_Burn(string mat_name)
    {
        Debug.Log(mat_name);
        PermissionPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        MessageHandler.Server_BurnMat(mat_name);
        
    }

    public void NoButton()
    {
        PermissionPanel.SetActive(false);
        MintText.SetActive(false);
        BurnText.SetActive(false);
    }

    public void OkayButton()
    {
        DonePanel.SetActive(false);
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
            foreach (MaterialDataModel m_data in mdata)
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
