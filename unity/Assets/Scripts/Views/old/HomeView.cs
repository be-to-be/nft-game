using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class HomeView : BaseView
{
    /*public TMP_Text username;
    public TMP_Text balance;

    public GameObject AssetData_Prefab;
    public GameObject RegPanel;
    public GameObject UnReg_Panel;
    public GameObject Content_Reg;
    public GameObject Content_UnReg;
    public GameObject ui_btn;

    private AssetModel[] assets;

    protected override void Start()
    {
        base.Start();
        MessageHandler.OnAssetData += OnAssetData;
        MessageHandler.OnNinjaData += OnNinjaData;
        MessageHandler.Server_GetAssetData();
        SetUIElements();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnAssetData -= OnAssetData;
        MessageHandler.OnNinjaData -= OnNinjaData;
    }

    private void SetUIElements()
    {
        if (MessageHandler.userModel.account != null)
        {
            username.text = MessageHandler.userModel.account;
            balance.text = MessageHandler.userModel.balance;
        }
    }

    private void OnAssetData(AssetModel[] assetModel)
    {
        LoadingPanel.SetActive(true);
        assets = assetModel;
        MessageHandler.Server_GetNinjaData();
    }

    private void OnNinjaData(NinjaDataModel[] ninjaModel)
    {
        LoadingPanel.SetActive(false);
        ui_btn.SetActive(true);

        for (int j=0; j < ninjaModel.Length; j++)
        {
            var ins = Instantiate(AssetData_Prefab);
            ins.transform.SetParent(Content_Reg.transform);
            var child = ins.gameObject.GetComponent<CitizenCall>();
            ins.gameObject.GetComponent<Button>().onClick.AddListener(child.searchforcitizen);
            child.asset_id = assets[j].asset_id;
            child.nft_img.url = "https://ipfs.wecan.dev/ipfs/" + assets[j].img;
            child.nft_name.text = assets[j].name;
            DateTime search_datetime = DateTime.Parse(ninjaModel[j].last_search);
            DateTime search_datetime_local = search_datetime.ToLocalTime();
            int compare_result = DateTime.Compare(DateTime.Now, search_datetime_local);

            if(compare_result >= 0)
            {
                child.timer_text.text = "Ready For Search Hunt!";
                child.search_status = true;
            }
            else if(compare_result < 0)
            {
                child.timer_text.text = "Next search at " + search_datetime;
                child.search_status = false;
            }

        }

        for (int i = 0; i < assets.Length; i++)
        {
            var ins = Instantiate(AssetData_Prefab);
            var child = ins.gameObject.GetComponent<CitizenCall>();
            child.asset_id = assets[i].asset_id;
            child.nft_img.url = "https://ipfs.wecan.dev/ipfs/" + assets[i].img;
            child.nft_name.text = assets[i].name;

            for (int j = 0; j < ninjaModel.Length; j++)
            {
                if (assets[i].asset_id == ninjaModel[j].asset_id)
                    Destroy(ins);

                else if(assets[i].asset_id != ninjaModel[j].asset_id && (j == ninjaModel.Length - 1))
                {
                    ins.transform.SetParent(Content_UnReg.transform);
                    ins.gameObject.GetComponent<Button>().onClick.AddListener(child.registernft);
                    string delay = ninjaModel[j].delay_seconds;
                    Debug.Log(delay);
                    child.timer_text.text = "Search Delay - " + delay + " sec";
                }
            }

        }

    }

    public void ViewReg_Assets()
    {
        UnReg_Panel.SetActive(false);
        RegPanel.SetActive(true);
    }

    public void ViewUnReg_Assets()
    {
        RegPanel.SetActive(false);
        UnReg_Panel.SetActive(true);
    }*/

}
