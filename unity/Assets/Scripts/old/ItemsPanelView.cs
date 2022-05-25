using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPanelView : MonoBehaviour
{
    public GameObject EquipButton;

    public string unequip_id;
    public string unequip_item_name;
    public string equip_id;
    public string p_id;
    // junks
    public Button Unequip_btn;
    public Button Equip_btn;
    public GameObject loadingPanel;

    public void PopupEquipButtonClick()
    {
        Debug.Log("PopupEquipButtonClick");
        Debug.Log(p_id);
        if (!string.IsNullOrEmpty(equip_id))
        {
            // loadingPanel.SetActive(true);
            MessageHandler.Server_EquipItems(p_id, equip_id);
        }
        else
            SSTools.ShowMessage("No item selected to equip", SSTools.Position.bottom, SSTools.Time.twoSecond);
    }


    // junk ones
    public void Equip_call()
    {
        Debug.Log(p_id);
        if (!string.IsNullOrEmpty(equip_id))
        {
            loadingPanel.SetActive(true);
            MessageHandler.Server_EquipItems(p_id, equip_id);
        }
        else
            SSTools.ShowMessage("No item selected to equip", SSTools.Position.bottom, SSTools.Time.twoSecond);
    }

    public void Unequip_call()
    {
        Debug.Log(unequip_id);
        if (!string.IsNullOrEmpty(unequip_id))
        {
            loadingPanel.SetActive(true);
            MessageHandler.Server_UnequipItems(unequip_id, unequip_item_name, p_id);
        }
        else
            SSTools.ShowMessage("No item selected to unequip", SSTools.Position.bottom, SSTools.Time.twoSecond);
    }

    public void BuyBtn()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=items");
    }

}
