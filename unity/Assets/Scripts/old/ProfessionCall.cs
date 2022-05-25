using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfessionCall : MonoBehaviour
{
    public RawImage img;
    public TMP_Text name_nft;
    public TMP_Text item_count;
    public TMP_Text timer;
    public TMP_Text uses_left;
    public TMP_Text refine_text;
    public TMP_Text craft_text;
    public GameObject Registered;
    public GameObject RefineBtn;
    public GameObject WorkBtn;
    public GameObject UnRegistered;
    public GameObject Timer;
    public GameObject Check;
    public GameObject CraftBtn;
    public GameObject ItemBtn;
    public GameObject Sellbtn;
    public Button Register_Btn;
    public Button UnRegister_Btn;
    public Button BurnBtn;
    public Transform BtnParent;
    public GameObject LoadingPanel;
    public string asset_ids;
    public string type;
    public bool gatherer = false;
    public string matName;
    public bool craft = false;

    public void RegisterBtn()
    {
        Debug.Log("Register");
        if (string.IsNullOrEmpty(asset_ids))
        {
            SSTools.ShowMessage("Please click on NFT to Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            LoadingPanel.SetActive(true);
            MessageHandler.Server_RegisterNFT(asset_ids, type);
        }
    }

    public void UnregisterBtn()
    {
        if (string.IsNullOrEmpty(asset_ids) || string.IsNullOrEmpty(type))
        {
            SSTools.ShowMessage("Please click on Registered NFT to De-Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            LoadingPanel.SetActive(true);
            MessageHandler.Server_UnregisterNFT(asset_ids, type);
        }
    }

    public void StartTimer(string last_search,int delay)
    {
        StartCoroutine(StartCountdown(last_search,delay));
    }

    public void CheckBtn_Call()
    {
        LoadingPanel.SetActive(true);
        if (gatherer) MessageHandler.Server_FindMat(asset_ids);
        else
        {
            if (!craft) MessageHandler.Server_RefineComp(asset_ids);
            else MessageHandler.Server_CraftComp(asset_ids);
        }
    }

    public void WorkBtn_Call()
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_SearchCitizen(asset_ids, "1",type);
    }

    public void SellBtn_Call()
    {
        Application.OpenURL("https://wax-test.atomichub.io/explorer/asset/" + asset_ids);
    }

    public void BurnBtn_Call()
    {
        if (!string.IsNullOrEmpty(asset_ids))
        {
            LoadingPanel.SetActive(true);
            MessageHandler.Server_BurnProfession(type,asset_ids);
        }
        else
        {
            SSTools.ShowMessage("Asset ID is null", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }

    private IEnumerator StartCountdown(string time,int delay)
    {
        ItemBtn.gameObject.GetComponent<Button>().interactable = false;
        Sellbtn.gameObject.GetComponent<Button>().interactable = false;
        CraftBtn.gameObject.GetComponent<Button>().interactable = false;
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int epoch_time = (int)(DateTime.Parse(time) - epochStart).TotalSeconds;
        int final_epoch_time = epoch_time + delay;
        int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        int diff = final_epoch_time - currentEpochTime;
        if (diff > 0)
        {
            Timer.SetActive(true);
            int temp = 0;
            while (temp != 1)
            {
                TimeSpan Ntime = TimeSpan.FromSeconds(diff);
                timer.text = Ntime.ToString();
                yield return new WaitForSeconds(1f);
                diff -= 1;
                if (diff == 0) temp = 1;
            }
        }

        Timer.SetActive(false);
        Check.SetActive(true);
    }

}
