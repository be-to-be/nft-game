using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NinjaShow : BaseView
{
    public TMP_Text username;
    public TMP_Text citizens;
    public TMP_Text professions;
    public TMP_Text materials;
    public TMP_Text ninjas;
    public TMP_Text[] ninja_each_count;
    //public TMP_Text ninja_name_text;
    public TMP_Text foundtext;
    // liu
    [Space]
    [Header("PanelManager")]
    public GameObject NinjaInfoPanel;
    public TMP_Text RegisterInfoText;
    public GameObject NinjsEmptyPanel;
    public GameObject ContentPanel;
    public Transform ContentTransform;
    public GameObject OneNinjaPrefab;
    public GameObject CampEmptyPanel;
    public GameObject OneCampPrefab;
    public GameObject FoundCzPopup;
    public GameObject NoFoundCzPopup;
    [Space]
    [Header("SceneAsset")]
    public ImgObjectView[] images;
    



    public GameObject textPanel;
    public GameObject showPanel;
    
    public GameObject foundPanel;
    public GameObject SettlementParentPanel;
    public GameObject SettlementTextPanel;
    public GameObject SettlementChildPanel;
    public GameObject UnregisteredSettlementChildPanel;
    public GameObject RegisteredSettlementChildPanel;
    public GameObject SettlementBuyBtn;
    public GameObject SettlementDeregButton;
    public GameObject SettlementPrefab;
    public GameObject NoCamp_text;
    public Transform parent1;
    
    public Transform settlementObj;
    public Transform unregisteredSettlementObj;
    public RawImage human;
    public RawImage elf;
    public RawImage orc;
    public RawImage undead;
    public RawImage demon;

    private List<NinjaDataModel> Human = new List<NinjaDataModel>();
    private List<NinjaDataModel> Orc = new List<NinjaDataModel>();
    private List<NinjaDataModel> Undead = new List<NinjaDataModel>();
    private List<NinjaDataModel> Elf = new List<NinjaDataModel>();
    private List<NinjaDataModel> Demon = new List<NinjaDataModel>();
    private List<SettlementsModel> Camp = new List<SettlementsModel>();

    private Dictionary<string, DelayDataModel> DelayValues = new Dictionary<string, DelayDataModel>();
    private Dictionary<string, string> imgHashes = new Dictionary<string, string>();
    
    // setting the header
    public delegate void SetHeader();
    public static SetHeader onSetHeaderElements;
    


    protected override void Start()
    {
        base.Start();
        SetModels(MessageHandler.userModel.ninjas);
        SetSettlementsModel(MessageHandler.userModel.settlements);
        SetUIElements();
        MessageHandler.OnNinjaData += OnNinjaData;
        MessageHandler.OnCallBackData += OnCallBackData;
        MessageHandler.OnSettlementData += OnSettlementData;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.OnNinjaData -= OnNinjaData;
        MessageHandler.OnCallBackData -= OnCallBackData;
        MessageHandler.OnSettlementData -= OnSettlementData;
    }
    private void SetUIElements()
    {
        if (MessageHandler.userModel.account != null)
        {
            RegisteredCountShow("ninja");
            foreach (DelayDataModel data in MessageHandler.userModel.config.race_delay_values)
            {
                if (!DelayValues.ContainsKey(data.key)) DelayValues.Add(data.key, data);
            }
            for (int i = 0; i < ninja_each_count.Length; i++)
            {
                switch (ninja_each_count[i].gameObject.name)
                {
                    case ("Human"):
                        ninja_each_count[i].text = "x" + Human.Count.ToString();
                        if (imgHashes.ContainsKey("Human")) human.gameObject.GetComponent<ImageLoader>().url = "https://ipfs.wecan.dev/ipfs/" + imgHashes["Human"];
                        break;
                    case ("Elf"):
                        ninja_each_count[i].text = "x" + Elf.Count.ToString();
                        if (imgHashes.ContainsKey("Elf")) human.gameObject.GetComponent<ImageLoader>().url = "https://ipfs.wecan.dev/ipfs/" + imgHashes["Elf"];
                        break;
                    case ("Orc"):
                        ninja_each_count[i].text = "x" + Orc.Count.ToString();
                        if (imgHashes.ContainsKey("Orc")) human.gameObject.GetComponent<ImageLoader>().url = "https://ipfs.wecan.dev/ipfs/" + imgHashes["Orc"];
                        break;
                    case ("Undead"):
                        ninja_each_count[i].text = "x" + Undead.Count.ToString();
                        if (imgHashes.ContainsKey("Undead")) human.gameObject.GetComponent<ImageLoader>().url = "https://ipfs.wecan.dev/ipfs/" + imgHashes["Undead"];
                        break;
                    case ("Demon"):
                        ninja_each_count[i].text = "x" + Demon.Count.ToString();
                        if (imgHashes.ContainsKey("Demon")) human.gameObject.GetComponent<ImageLoader>().url = "https://ipfs.wecan.dev/ipfs/" + imgHashes["Demon"];
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void SetModels(NinjaDataModel[] ninjas)
    {
        Human.Clear();
        Elf.Clear();
        Orc.Clear();
        Undead.Clear();
        Demon.Clear();

        foreach (NinjaDataModel ninja in ninjas)
        {
            switch (ninja.race)
            {
                case ("Human"):
                    Human.Add(ninja);
                    break;
                case ("Elf"):
                    Elf.Add(ninja);
                    break;
                case ("Orc"):
                    Orc.Add(ninja);
                    break;
                case ("Undead"):
                    Undead.Add(ninja);
                    break;
                case ("Demon"):
                    Demon.Add(ninja);
                    break;
                default:
                    break;
            }
        }
    }

    private void SetSettlementsModel(SettlementsModel[] settlements)
    {
        Camp.Clear();
        foreach (SettlementsModel data in settlements)
        {
            switch (data.name)
            {
                case ("Camp"):
                    Camp.Add(data);
                    break;
                default:
                    break;
            }
        }
    }

    public void NinjaButtonClick(string name)
    {
        NinjaInfoPanel.SetActive(false);
        switch (name)
        {
            case ("Human"):
                NinjaActionShow(Human);
                break;
            case ("Elf"):
                NinjaActionShow(Elf);
                break;
            case ("Orc"):
                NinjaActionShow(Orc);
                break;
            case ("Undead"):
                NinjaActionShow(Undead);
                break;
            case ("Demon"):
                NinjaActionShow(Demon);
                break;
            default:
                break;
        }
    }
    public void RegisteredCountShow(string type){
        if (type == "ninja")
        {
            string maxCount = "10";
            foreach (MaxNftDataModel nftData in MessageHandler.userModel.nft_count)
            {
                if (nftData.name == "Max Ninja")
                {
                    maxCount = nftData.count;
                    break;
                }
            }
            
            int registeredCount = 0;
            foreach (NinjaDataModel a in MessageHandler.userModel.ninjas)
            {
                if (a.reg == "1")
                {
                    registeredCount += 1;
                }
            }
            RegisterInfoText.text = "Waxel Ninjas " + registeredCount.ToString() + "/" + maxCount;
        }
        else
        {
            string maxCount = Camp.Count.ToString();            
            int registeredCount = 0;
            foreach (SettlementsModel c in Camp)
            {
                if (c.reg == "1")
                {
                    registeredCount += 1;
                }
            }
            RegisterInfoText.text = "Camp " + registeredCount.ToString() + "/" + maxCount;
        }

    }
    public void NinjaActionShow(List<NinjaDataModel> ninjaModel)
    {   
        RegisteredCountShow("ninja");
        if (ninjaModel.Count > 0)
        {
            NinjsEmptyPanel.SetActive(false);
            ContentPanel.SetActive(true);
            if (ContentTransform.childCount >= 1)
            {
                foreach (Transform child in ContentTransform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            foreach (NinjaDataModel ninja in ninjaModel)
            {
                var ins = Instantiate(OneNinjaPrefab);
                ins.transform.SetParent(ContentTransform);
                ins.transform.localScale = new Vector3(1, 1, 1);
                var child = ins.gameObject.GetComponent<NinjaStatus>();
                child.race = ninja.race;
                child.assetId = ninja.asset_id;
                child.name.text = "Waxel Ninja #" + ninja.mint_id;
                // child.img.loadimg("https://ipfs.wecan.dev/ipfs/" + ninja.img);
                int temp = 0;
                for (int j = 0; j < images.Length; j++)
                {
                    if (images[j].name == child.race)
                    {
                        temp = j;
                        break;
                    }
                }
                child.img.texture = images[temp].img;
                //child.LoadingPanel = LoadingPanel;
                if (ninja.reg == "0")
                {
                    child.Register.SetActive(true);
                }
                else
                {
                    child.SellBtn.gameObject.GetComponent<Button>().interactable = false;
                    if (ninja.last_search != "1970-01-01T00:00:00")
                    {
                        switch (ninja.status)
                        {
                            case ("Searching"):
                                child.StartTimer(ninja.last_search, DelayValues[ninja.race].value);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (ninja.last_search == "1970-01-01T00:00:00")
                    {
                        child.Registered.SetActive(true);
                    }
                }
            }
        } 
        else
        {
            ContentPanel.SetActive(false);
            NinjsEmptyPanel.SetActive(true);
        }
    }

    public void ShowSettlements(List<SettlementsModel> camp)
    {
        RegisteredCountShow("camp");
        ContentPanel.SetActive(true);
        // NinjaInfoPanel.SetActive(false);
        // CampEmptyPanel.SetActive(false);
        // NinjsEmptyPanel.SetActive(false);
        // NinjasByNamePanel.SetActive(true);
        if (ContentTransform.childCount >= 1)
        {
            foreach (Transform child in ContentTransform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        foreach (SettlementsModel data in camp)
        {
            var ins = Instantiate(OneCampPrefab);
            ins.transform.SetParent(ContentTransform);
            ins.transform.localScale = new Vector3(1, 1, 1);
            var child = ins.gameObject.GetComponent<OneCampStatus>();
            child.name.text = "#" + data.asset_id;
            // // child.nft_name.text = "Name : " + data.name;
            child.assetId = data.asset_id;
            // // child.asset_id = data.asset_id;
            // child.img.loadimg("https://ipfs.wecan.dev/ipfs/" + data.img);
            // // child.nftImg.loadimg("https://ipfs.wecan.dev/ipfs/" + data.img);
            if (data.reg == "0")
            { 
                // ins.transform.SetParent(unregisteredSettlementObj);
                // ins.transform.localScale = new Vector3(1, 1, 1);
                child.Register.SetActive(true);
                // child.Register.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Register_Settlement(data.asset_id); });
            }
            else if (data.reg == "1")
            { 
                child.SellBtn.gameObject.GetComponent<Button>().interactable = false;
                child.Unregister.SetActive(true);
                // child.DeRegister.gameObject.GetComponent<Button>().onClick.AddListener(delegate { DeRegister_Settlement(data.asset_id); });
            }
        }
    }

    public void Register_Settlement(string asset_id)
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_TransferAsset(asset_id, "regupgrade","Camp");
    }

    public void DeRegister_Settlement(string asset_id)
    {
        LoadingPanel.SetActive(true);
        MessageHandler.Server_WithdrawAsset(asset_id,"Camp");
    }

    public void okButton()
    {
        foundPanel.SetActive(false);
    }

    public void OnNinjaData(NinjaDataModel[] ninjas)
    {
        SetModels(ninjas);
    }

    public void OnSettlementData(SettlementsModel[] settlements)
    {
        SetSettlementsModel(settlements);
    }

    public void OnCallBackData(CallBackDataModel[] callback)
    {
        CallBackDataModel callBack = callback[0];
        switch (callBack.name)
        {
            case ("Human"):
                NinjaActionShow(Human);
                break;
            case ("Elf"):
                NinjaActionShow(Elf);
                break;
            case ("Orc"):
                NinjaActionShow(Orc);
                break;
            case ("Undead"):
                NinjaActionShow(Undead);
                break;
            case ("Demon"):
                NinjaActionShow(Demon);
                break;
            case ("Camp"):
                ShowSettlements(Camp);
                break;
            default:
                break;
        }
        //LoadingPanel.SetActive(false);
        switch (callBack.status)
        {
            case ("Search successful"):
                MessageHandler.userModel.citizens = callBack.totalCitizensCount;
                // citizens.text = MessageHandler.userModel.citizens;
                FoundCzPopup.SetActive(true);
                NoFoundCzPopup.SetActive(false);
                break;
            case ("Search failed"):
                NoFoundCzPopup.SetActive(true);
                FoundCzPopup.SetActive(false);
                break;
            case ("Registered Successfully"):
                SSTools.ShowMessage("NFT Registration Successful !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            case ("De-Registered Successfully"):
                SSTools.ShowMessage("NFT De-Registration Successful !", SSTools.Position.bottom, SSTools.Time.twoSecond);
                break;
            default:
                break;
        }
    }

    public void BuyBtn()
    {
        Application.OpenURL(MessageHandler.userModel.drop);
    }
    public void FoundCzPopupOkayButtonClick()
    {
        onSetHeaderElements();
    }

    public void BuyUpgrades()
    {
        Application.OpenURL("https://wax-test.atomichub.io/market?collection_name=laxewneftyyy&schema_name=upgrades&template_id=282656");
    }


    public void CrossBtn()
    {
        SettlementParentPanel.SetActive(false);
        SettlementChildPanel.SetActive(false);
        UnregisteredSettlementChildPanel.SetActive(false);
        SettlementTextPanel.SetActive(false);
        SettlementBuyBtn.SetActive(false);
        SettlementDeregButton.SetActive(false);
        RegisteredSettlementChildPanel.SetActive(false);
        NoCamp_text.SetActive(true);
    }

    public void UpgradeButtonClick()
    {
        if(Camp.Count < 1){
            CampEmptyPanel.SetActive(true);
        } else {
            NinjaInfoPanel.SetActive(false);
            NinjsEmptyPanel.SetActive(false);
            ShowSettlements(Camp);
        }

    }

}
