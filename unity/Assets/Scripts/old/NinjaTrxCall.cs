using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NinjaTrxCall : MonoBehaviour
{
    public ImageLoader img;
    public TMP_Text ninja_name;
    public TMP_Text timer;
    public string asset_ids;
    public string race;

    public GameObject Registered;
    public GameObject UnRegistered;
    public GameObject Timer;
    public GameObject Check;
    public Button SellBtn;
    public Button RegisterBtn;
    public Button UnRegisterBtn;
    public Button CheckBtn;
    public Button SearchBtn;
    public GameObject LoadingPanel;

    public void SellButton()
    {
        Application.OpenURL("https://wax-test.atomichub.io/explorer/asset/" + asset_ids);
    }

    public void CheckBtn_Call()
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_SearchCitizen(asset_ids, "2",race);
    }

    public void RegisterBtn_Call()
    {
        if (string.IsNullOrEmpty(asset_ids))
        {
            SSTools.ShowMessage("Please click on NFT to Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            LoadingPanel.SetActive(true);
            MessageHandler.Server_RegisterNFT(asset_ids, race);
        }
    }

    public void UnregisterBtn_Call()
    {
        if (string.IsNullOrEmpty(asset_ids) || string.IsNullOrEmpty(race))
        {
            SSTools.ShowMessage("Please click on Registered NFT to De-Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            LoadingPanel.SetActive(true);
            MessageHandler.Server_UnregisterNFT(asset_ids, race);
        }
    }

    public void SearchCtz()
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_SearchCitizen(asset_ids, "1",race);
    }

    public void StartTimer(string last_search,string delayValue)
    {
        Debug.Log(delayValue);
        StartCoroutine(StartCountdown(last_search,delayValue));
    }

    private IEnumerator StartCountdown(string time,string delayValue)
    {
        Debug.Log("In timer");
        SellBtn.gameObject.GetComponent<Button>().interactable = false;
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int epoch_time = (int)(DateTime.Parse(time) - epochStart).TotalSeconds;
        Debug.Log(epoch_time);
        double delay_seconds = Convert.ToDouble(delayValue);
        Debug.Log(delay_seconds);
        double final_epoch_time = epoch_time + delay_seconds;
        double currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        double diff = final_epoch_time - currentEpochTime;
        Debug.Log(diff);
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
