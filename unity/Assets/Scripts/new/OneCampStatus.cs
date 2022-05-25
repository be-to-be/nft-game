using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneCampStatus : MonoBehaviour
{
    public ImageLoader img;
    public TMP_Text name;
    //public GameObject UnRegistered;
    public GameObject Register;
    public GameObject Unregister;
    // public TMP_Text timer;
    // public GameObject Timer;
    // public GameObject Check;
    public GameObject SellBtn;

    public string assetId;
    // public string race;

    public void RegisterButtonClick()
    {
        MessageHandler.Server_TransferAsset(assetId, "regupgrade","Camp");
    }
        public void UnregisterButtonClick()
    {
        MessageHandler.Server_WithdrawAsset(assetId, "Camp");
    }
}
