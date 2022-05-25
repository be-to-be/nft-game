using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneSettlementStatus : MonoBehaviour
{
    public RawImage image;
    public TMP_Text IdText;
    public GameObject Register;
    public GameObject Unregister;
    public GameObject SellBtn;

    public string assetId;
    public string type;

    public void RegisterButtonClick()
    {
        MessageHandler.Server_TransferAsset(assetId, "regupgrade", type);
    }
    public void UnregisterButtonClick()
    {
        MessageHandler.Server_WithdrawAsset(assetId, type);
    }
    public void SellButtonClick()
    {
        MessageHandler.Server_WithdrawAsset(assetId, type);
    }
}
