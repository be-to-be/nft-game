using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CitizenShow : BaseView
{
    public GameObject MintConfirmPanel;
    public GameObject MintSuccessPanel;


    public GameObject PermissionPanel;
    public GameObject DonePanel;
    public GameObject DonePanel_Obj;
    public GameObject Loader;
    public GameObject MintText;
    public GameObject BurnText;
    public Button SellBtn;
    public TextMeshProUGUI Text;

    public TMP_Text username;
    public TMP_Text citizens;
    public TMP_Text professions;
    public TMP_Text materials;
    public TMP_Text ninjas;
    public TMP_Text done_panel_text;
    

    protected override void Start()
    {
        base.Start();
        MessageHandler.OnTransactionData += OnTransactionData;
        SetUIElements();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnTransactionData -= OnTransactionData;
    }

    private void SetUIElements()
    {
        // if (MessageHandler.userModel.account != null)
        // {
        //     username.text = MessageHandler.userModel.account;
        //     citizens.text = MessageHandler.userModel.citizens;
        //     professions.text = MessageHandler.userModel.professions.Length.ToString();
        //     materials.text = MessageHandler.userModel.items.Length.ToString();
        //     ninjas.text = MessageHandler.userModel.ninjas.Length.ToString();
        //     foreach(AssetModel data in MessageHandler.userModel.assets)
        //     {
        //         if(data.name == "Citizens - 10x" && data.schema == "citizens")
        //         {
        //             SellBtn.interactable = true;
        //             break;
        //         }
        //     }
        // }
    }

    public void MintButtonClick()
    {
        // if (BurnText.activeInHierarchy) BurnText.SetActive(false);
        if (Int64.Parse(MessageHandler.userModel.citizens) >= 10)
        {
            MintConfirmPanel.SetActive(true);

            // PermissionPanel.SetActive(true);
            // MintText.SetActive(true);
        }
        else if(Int64.Parse(MessageHandler.userModel.citizens) < 10)
        {
            SSTools.ShowMessage("Your Citizen's Balance is Less than 10",SSTools.Position.bottom,SSTools.Time.threeSecond);
        }

    }

    public void SellButton()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=citizens&template_id=263183");
    }

    public void AddButtonClick()
    {
        PermissionPanel.SetActive(true);
        if (MintText.activeInHierarchy)
            MintText.SetActive(false);
        BurnText.SetActive(true);
    }

    public void BuyButton()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=citizens&template_id=263183");
    }

    public void MintYesButtonClick()
    {
        // if (MintText.activeInHierarchy)
        // {
            // PermissionPanel.SetActive(false);
            MintConfirmPanel.SetActive(false);
            // Loader.SetActive(true);
            MessageHandler.Server_MintCitizenPack();
        // }
        // else if (BurnText.activeInHierarchy)
        // {
        //     PermissionPanel.SetActive(false);
        //     Loader.SetActive(true);
        //     MessageHandler.Server_BurnCitizenPack();
        // }
    }

    public void NoButton()
    {
        PermissionPanel.SetActive(false);
    }

    public void OkayButton()
    {
       DonePanel.SetActive(false);
       MintText.SetActive(false);
       BurnText.SetActive(false);
    }

    public void SelectProfession_Btn()
    {
        Loader.SetActive(true);
        SceneManager.LoadScene("ProfessionScene");
    }

    private void OnTransactionData()
    {
        if (MessageHandler.transactionModel.transactionid != "")
        {
            // Loader.SetActive(false);
            // DonePanel.SetActive(true);
            // DonePanel_Obj.SetActive(true);

            if (MessageHandler.transactionModel.transactionid == "Mint")
            {
                // done_panel_text.text = "The Citizens - 10x NFT was added to your wallet";
                MintSuccessPanel.SetActive(true);
                MessageHandler.userModel.citizens = MessageHandler.transactionModel.citizens;
                // citizens.text = MessageHandler.userModel.citizens;

            }
            if (MessageHandler.transactionModel.transactionid == "Burn") 
            { 
                done_panel_text.text = "10 Citizens have been added to your account";
                MessageHandler.userModel.citizens = MessageHandler.transactionModel.citizens;
                citizens.text = MessageHandler.userModel.citizens;
            }

        }
    }
}
