using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CampShow : BaseView
{
    public GameObject MintConfirmPopup;
    public GameObject MintSuccessPopup;
    public GameObject NoCitizenToMintPopup;
    public GameObject AddConfirmPopup;

    public GameObject AddSucessPopup;

    public GameObject NoCitizenToAddPopup;
    
    // setting the header
    public delegate void SetHeader();
    public static SetHeader onSetHeaderElements;

    protected override void Start()
    {
        base.Start();
        MessageHandler.OnTransactionData += OnTransactionData;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnTransactionData -= OnTransactionData;
    }
    public void MintButtonClick()
    {
        if (Int64.Parse(MessageHandler.userModel.citizens) >= 10)
        {
            MintConfirmPopup.SetActive(true);
        }
        else if(Int64.Parse(MessageHandler.userModel.citizens) < 10)
        {
            NoCitizenToMintPopup.SetActive(true);
        }
    }
    public void AddButtonClick()
    {
        if (MessageHandler.userModel.citizens_pack_count > 0)
        {
            AddConfirmPopup.SetActive(true);
        }
        else
        {
            NoCitizenToAddPopup.SetActive(true);
        }
    }
    public void MintYesButtonClick()
    {
        MintConfirmPopup.SetActive(false);
        MessageHandler.Server_MintCitizenPack();
    }
    public void AddYesButtonClick()
    {
        AddConfirmPopup.SetActive(false);
        MessageHandler.Server_BurnCitizenPack();
    }
    private void OnTransactionData()
    {
        if (MessageHandler.transactionModel.transactionid != "")
        {
            if (MessageHandler.transactionModel.transactionid == "Mint")
            {
                MintSuccessPopup.SetActive(true);
                MessageHandler.userModel.citizens = MessageHandler.transactionModel.citizens;
                MessageHandler.userModel.citizens_pack_count = MessageHandler.transactionModel.citizens_pack_count;
                // MessageHandler.userModel.citizens = callBack.totalCitizensCount;
            }
            if (MessageHandler.transactionModel.transactionid == "Burn") 
            { 
                AddSucessPopup.SetActive(true);
                MessageHandler.userModel.citizens = MessageHandler.transactionModel.citizens;
                MessageHandler.userModel.citizens_pack_count = MessageHandler.transactionModel.citizens_pack_count;

            }
            onSetHeaderElements();
        }
    }
}
