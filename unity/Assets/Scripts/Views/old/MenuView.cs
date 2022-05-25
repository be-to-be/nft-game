using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class MenuView : BaseView
{
    public TMP_Text username;
    public TMP_Text citizens;
    public TMP_Text professions;
    public TMP_Text materials;
    public TMP_Text ninjas;

    protected override void Start()
    {
        base.Start();
        SetUIElements();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void SetUIElements()
    {
        if (MessageHandler.userModel.account != null)
        {
            Debug.Log("Professions Length -    " + MessageHandler.userModel.professions.Length);
            Debug.Log("Ninjas Length -    " + MessageHandler.userModel.ninjas.Length);
            Debug.Log("Material Length -    " + MessageHandler.userModel.items.Length);
            username.text = MessageHandler.userModel.account;
            citizens.text = MessageHandler.userModel.citizens;
            professions.text = MessageHandler.userModel.professions.Length.ToString();
            materials.text = MessageHandler.userModel.items.Length.ToString();
            ninjas.text = MessageHandler.userModel.ninjas.Length.ToString();
        }
    }

    public void profession_btn()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("ProfessionScene");
    }

    public void citizen_btn()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("CitizensScene");
    }

    public void material_btn()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("MaterialsScene");
    }

    public void ninjas_btn()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("NinjaScene");
    }

    public void market_btn()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy");
    }

    public void shop_btn()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("ShopScene");
    }

    public void workshop_btn()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("WorkshopScene");
    }

}
