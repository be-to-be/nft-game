using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCall : MonoBehaviour
{
    public Image img;
    public TMP_Text asset_ids;
    public TMP_Text durability;
    public GameObject equip;
    public GameObject unequip;
    public GameObject burn;
    public GameObject sell;
    public string asset_id;
    public string mat_name;
    public GameObject LoadingPanel;
    public string p_id;

    public void UnequipItem()
    {
        if (string.IsNullOrEmpty(asset_id))
        {
            SSTools.ShowMessage("Please click on Item to Unequip", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
        else
        {
            Debug.Log(p_id);
            if (!string.IsNullOrEmpty(p_id))
            {
                LoadingPanel.SetActive(true);
                MessageHandler.Server_UnequipItems(asset_id, mat_name, p_id);
            }
            else
            {
                SSTools.ShowMessage("Profession ID is null", SSTools.Position.bottom, SSTools.Time.twoSecond);
            }
        }
    }

    public void SellBtn()
    {
        Application.OpenURL("https://wax-test.atomichub.io/explorer/asset/" + asset_id);
    }

    public void BurnBtn()
    {
        if (!string.IsNullOrEmpty(asset_id))
        {
            LoadingPanel.SetActive(true);
            MessageHandler.Server_BurnItem(mat_name, asset_id);
        }
        else
        {
            SSTools.ShowMessage("Asset ID is null", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }

}
