using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
public class CitizenCall : MonoBehaviour
{
    public string asset_id;
    public ImageLoader nft_img;
    public TMP_Text nft_name;
    public TMP_Text timer_text;
    public bool search_status
    {
        set 
        {
            this.gameObject.GetComponent<Button>().interactable = value;
        }
    }

    public void searchforcitizen()
    {
        //GameObject.Find("GameController").GetComponent<MessageHandler>().Client_SearchCitizen(this.gameObject.GetComponent<CitizenCall>().asset_id);
    }

    public void registernft()
    {
        //GameObject.Find("GameController").GetComponent<MessageHandler>().Client_RegisterNFT(this.gameObject.GetComponent<CitizenCall>().asset_id);
    }
}
