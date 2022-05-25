using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Header : BaseView
{
    public TMP_Text username;
    public TMP_Text citizens;
    public TMP_Text professions;
    public TMP_Text materials;
    public TMP_Text ninjas;
    public TMP_Text items;
    protected override void Start()
    {
        base.Start();
        NinjaShow.onSetHeaderElements += onSetHeaderElements;
        CampShow.onSetHeaderElements += onSetHeaderElements;
        onSetHeaderElements();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        NinjaShow.onSetHeaderElements -= onSetHeaderElements;
        CampShow.onSetHeaderElements -= onSetHeaderElements;

    }
    private void onSetHeaderElements()
    {
        if (MessageHandler.userModel.account != null)
        {
            ninjas.text = MessageHandler.userModel.ninjas.Length.ToString();
            citizens.text = MessageHandler.userModel.citizens;
            professions.text = MessageHandler.userModel.professions.Length.ToString();
            materials.text = Int64.Parse(MessageHandler.userModel.total_matCount).ToString();
            items.text = MessageHandler.userModel.items.Length.ToString();
            username.text = MessageHandler.userModel.account;
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
