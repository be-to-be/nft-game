using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneProfessionStatus : MonoBehaviour
{
    public RawImage img;
    public TMP_Text name_nft;
    public TMP_Text item_count;
    public TMP_Text uses_left;
    public TMP_Text refine_text;
    public TMP_Text craft_text;
    public GameObject RefineBtn;
    public GameObject WorkBtn;
    [Space]
    [Header("MyButtons")]
    public GameObject Register;
    public GameObject Registered;
    public GameObject ActionBtn;

    public TMP_Text action_text;
    public GameObject Timer;
    public TMP_Text timer;
    public GameObject Check;
    public GameObject Seller;
    public GameObject ItemSeller;
    public GameObject ItemBtn;
    public TMP_Text ItemInfo;
    [Space]
    [Header("MyProperty")]
    public string assetId;
    public TMP_Text AssetIdText;
    public string type;
    public TMP_Text UseLeftCount;





    public GameObject CraftBtn;

    public Button Register_Btn;
    public Button UnRegister_Btn;
    public Button BurnBtn;
    public Transform BtnParent;
    public GameObject LoadingPanel;
    

    public bool gatherer = false;
    public string matName;
    public bool craft = false;

    public void RegisterButtonClick()
    {
        // Debug.Log("Register");
        if (string.IsNullOrEmpty(assetId))
        {
            SSTools.ShowMessage("Please click on NFT to Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            //LoadingPanel.SetActive(true);
            MessageHandler.RegisterNFTRequest(assetId, type);
        }
    }

    public void UnregisterButtonClick()
    {
        if (string.IsNullOrEmpty(assetId) || string.IsNullOrEmpty(type))
        {
            SSTools.ShowMessage("Please click on Registered NFT to De-Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            //LoadingPanel.SetActive(true);
            MessageHandler.Server_UnregisterNFT(assetId, type);
        }
    }

    public void StartTimer(string last_search,int delay)
    {
        StartCoroutine(StartCountdown(last_search, delay));
    }

    public void CheckButtonClick()
    {   
        switch (type)
        {
            case "Miner":
            case "Farmer":
            case "Lumberjack":
                    MessageHandler.Server_FindMat(assetId);
                break;
            case "Engineer":
                    MessageHandler.Server_CraftComp(assetId);
                break;
            case "Carpenter":
            case "Tailor":
            case "Blacksmith":
                    MessageHandler.Server_RefineComp(assetId);
                break;
            default:
                break;
        }
        // LoadingPanel.SetActive(true);
        // if (gatherer) MessageHandler.Server_FindMat(assetId);
        // else
        // {
        //     if (!craft) MessageHandler.Server_RefineComp(assetId);
        //     else MessageHandler.Server_CraftComp(assetId);
        // }
    }

    public void WorkBtn_Call()
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_SearchCitizen(assetId, "1", type);
    }

    public void SellerButtonClick()
    {
        Application.OpenURL("https://wax-test.atomichub.io/explorer/asset/" + assetId);
    }

    public void BurnBtn_Call()
    {
        if (!string.IsNullOrEmpty(assetId))
        {
            LoadingPanel.SetActive(true);
            MessageHandler.Server_BurnProfession(type,assetId);
        }
        else
        {
            SSTools.ShowMessage("Asset ID is null", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }

    private IEnumerator StartCountdown(string time,int delay)
    {
        // ItemBtn.gameObject.GetComponent<Button>().interactable = false;
        // Seller.gameObject.GetComponent<Button>().interactable = false;  
        // CraftBtn.gameObject.GetComponent<Button>().interactable = false;
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
