using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NinjaStatus : MonoBehaviour
{
    public RawImage img;
    public TMP_Text name;
    //public GameObject UnRegistered;
    public GameObject Register;
    public GameObject Registered;
    public TMP_Text timer;
    public GameObject Timer;
    public GameObject Check;
    public GameObject SellBtn;
    // public GameObject LoadingPanel;
    public string assetId;
    public string race;


    public void SellButtonClick(string url)
    {
        Application.OpenURL(url);
        // Application.OpenURL("https://wax-test.atomichub.io/explorer/asset/" + assetId);    
    }

    public void CheckButtonClick()
    {
        //LoadingPanel.SetActive(true);
        MessageHandler.Server_SearchCitizen(assetId, "2",race);
    }

    public void RegisterNinjaButtonClick()
    {
        if (string.IsNullOrEmpty(assetId))
        {
            SSTools.ShowMessage("Please click on NFT to Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            // LoadingPanel.SetActive(true);
            MessageHandler.RegisterNFTRequest(assetId, race);
        }
    }

    public void UnregisterButtonClick()
    {
        if (string.IsNullOrEmpty(assetId) || string.IsNullOrEmpty(race))
        {
            SSTools.ShowMessage("Please click on Registered NFT to De-Register", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            // LoadingPanel.SetActive(true);
            MessageHandler.Server_UnregisterNFT(assetId, race);
        }
    }

    public void SearchButtonClick()
    {
        // LoadingPanel.SetActive(true);
        MessageHandler.Server_SearchCitizen(assetId, "1",race);
    }

    public void StartTimer(string last_search,string delayValue)
    {
        // Debug.Log(delayValue);
        StartCoroutine(StartCountdown(last_search,delayValue));
    }

    private IEnumerator StartCountdown(string time,string delayValue)
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int epoch_time = (int)(DateTime.Parse(time) - epochStart).TotalSeconds;
        // Debug.Log(epoch_time);
        double delay_seconds = Convert.ToDouble(delayValue);
        // Debug.Log(delay_seconds);
        double final_epoch_time = epoch_time + delay_seconds;
        double currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        double diff = final_epoch_time - currentEpochTime;
        // Debug.Log(diff);
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
