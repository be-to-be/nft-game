using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    // Request Handlers from Unity
    [DllImport("__Internal")]
    private static extern void autologin();
    [DllImport("__Internal")]
    private static extern void login(string type);
    [DllImport("__Internal")]
    private static extern void logout();
    [DllImport("__Internal")]
    private static extern void getAssetData();
    [DllImport("__Internal")]
    private static extern void mintcitizens();
    [DllImport("__Internal")]
    private static extern void burncitizennft();
    [DllImport("__Internal")]
    private static extern void getNinjaData();
    [DllImport("__Internal")]
    private static extern void searchcz(string assetid, string type,string asset_type);
    [DllImport("__Internal")]
    private static extern void registernft(string assetid, string race);
    [DllImport("__Internal")]
    private static extern void unregisternft(string assetid, string race);
    [DllImport("__Internal")]
    private static extern void asset_transfer(string assetid, string memo,string type);
    [DllImport("__Internal")]
    private static extern void asset_withdraw(string assetid,string type);
    [DllImport("__Internal")]
    private static extern void equip_items(string p_id,string item_id);
    [DllImport("__Internal")]
    private static extern void unequip_items(string ids,string mat_name,string p_id);
    [DllImport("__Internal")]
    private static extern void findmat(string asset_id);
    [DllImport("__Internal")]
    private static extern void refine_mat(string profession_id, string mat,string profession_name);
    [DllImport("__Internal")]
    private static extern void refine_comp(string profession_id);
    [DllImport("__Internal")]
    private static extern void craft_mat(string profession_id, string mat_id, string profession_name);
    [DllImport("__Internal")]
    private static extern void craft_comp(string profession_id);
    [DllImport("__Internal")]
    private static extern void mint_mat(string mat_name);
    [DllImport("__Internal")]
    private static extern void burn_mat(string mat_name);
    [DllImport("__Internal")]
    private static extern void burn_items(string item_name, string item_id);
    [DllImport("__Internal")]
    private static extern void burn_profession(string p_name, string p_id);


    public delegate void LoadingData(string status);
    public static LoadingData onLoadingData;

    public delegate void OnLogin();
    public static OnLogin OnLoginData;

    public delegate void CallBack(CallBackDataModel[] callData);
    public static CallBack OnCallBackData;

    public delegate void AssetData(AssetModel[] assetModel);
    public static AssetData OnAssetData;

    public delegate void InventoryData(InventoryModel[] inventoryData);
    public static InventoryData OnInventoryData;

    public delegate void NinjaData(NinjaDataModel[] ninjaData);
    public static NinjaData OnNinjaData;

    public delegate void SettlementData(SettlementsModel[] settlementData);
    public static SettlementData OnSettlementData;

    public delegate void ProfessionData(ProfessionDataModel[] professionData);
    public static ProfessionData OnProfessionData;

    public delegate void ItemData();
    public static ItemData OnItemData;

    public delegate void MaxNfts(MaxNftDataModel[] maxData);
    public static MaxNfts OnMaxNfts;

    public delegate void TransactionData();
    public static TransactionData OnTransactionData;

    public static LoadingModel loadingModel = null;
    public static UserModel userModel = null;
    public static CallBackDataModel[] callBack = null;
    public static AssetModel[] assetModel = null;
    public static InventoryModel[] inventoryData = null;
    public static NinjaDataModel[] ninjaData = null;
    public static ProfessionDataModel[] professionData = null;
    public static ItemDataModel[] itemData = null;
    public static MaxNftDataModel[] maxData = null;
    public static TransactionModel transactionModel = null;

    public static MessageHandler instance;

    public delegate void MessageData(string[] messages);
    public static MessageData OnMessageData;

    // Request from Unity
    public static void AutoLoginRequest()
    {
        autologin();
    }
    public static void LoginRequest(string type)
    {
        login(type);
    }
    public static void LogoutRequest()
    {
        logout();
    }
    public static void RegisterNFTRequest(string assetId, string race)
    {
        registernft(assetId, race);
    }
    public static void Server_RegisterNFT(string assetid, string race)
    {
        registernft(assetid, race);
    }
    public static void Server_SearchCitizen(string assetid, string type,string asset_type)
    {
        searchcz(assetid, type,asset_type);
    }
    public static void Server_UnregisterNFT(string assetid, string race)
    {
        unregisternft(assetid, race);
    }

    // Response to Unity
    public void ResponseCallbackData(string data)
    {
        string jsonData = JsonHelper.fixJson(data);
        callBack = JsonHelper.FromJson<CallBackDataModel>(jsonData);
        OnCallBackData(callBack);
    }
    public void Client_TrxHash(string trx)
    {
        transactionModel = JsonUtility.FromJson<TransactionModel>(trx);
        OnTransactionData();
    }
    public void Client_SetCallBackData(string data)
    {
        string jsonData = JsonHelper.fixJson(data);
        callBack = JsonHelper.FromJson<CallBackDataModel>(jsonData);
        OnCallBackData(callBack);
    }





    
    public void Client_SetNinjaData(string ninjadata)
    {
        string jsonData = JsonHelper.fixJson(ninjadata);
        NinjaDataModel[] ninja_data = JsonHelper.FromJson<NinjaDataModel>(jsonData);
        userModel.ninjas = ninja_data;
        OnNinjaData(ninja_data);
    }

    public static void Server_GetAssetData()
    {
        getAssetData();
    }

    public static void Server_GetNinjaData()
    {
        getNinjaData();
    }

    public static void Server_MintCitizenPack()
    {
        mintcitizens();
    }

    public static void Server_MintMat(string material_name,string amount)
    {
        mint_mat(material_name);
    }

    public static void Server_BurnMat(string material_name)
    {
        burn_mat(material_name);
    }

    public static void Server_BurnItem(string item_name,string item_id)
    {
        burn_items(item_name,item_id);
    }

    public static void Server_BurnProfession(string profession_name, string asset_id)
    {
        burn_profession(profession_name, asset_id);
    }






    public static void Server_BurnCitizenPack()
    {
        burncitizennft();
    }

    public static void Server_TransferAsset(string assetid,string memo,string type)
    {
        asset_transfer(assetid, memo,type);
    }

    public static void Server_WithdrawAsset(string assetid,string type)
    {
        asset_withdraw(assetid,type);
    }

    public static void Server_EquipItems(string p_id,string item_id)
    {
        equip_items(p_id, item_id);
    }

    public static void Server_UnequipItems(string item_id,string mat_name,string p_id)
    {
        unequip_items(item_id,mat_name,p_id);
    }

    public static void Server_FindMat(string asset_id)
    {
        findmat(asset_id);
    }

    public static void Server_RefineMat(string profession_id,string mat,string profession_name)
    {
        refine_mat(profession_id, mat, profession_name);
    }

    public static void Server_RefineComp(string profession_id)
    {
        refine_comp(profession_id);
    }

    public static void Server_CraftMat(string profession_id, string mat_template, string profession_name)
    {
        craft_mat(profession_id, mat_template, profession_name);
    }

    public static void Server_CraftComp(string profession_id)
    {
        craft_comp(profession_id);
    }

    public void Client_SetLoginData(string playerdata)
    {
        userModel = JsonUtility.FromJson<UserModel>(playerdata);
        OnLoginData();
    }

    public void Client_SetFetchingData(string status)
    {
        Debug.Log("In Unity " + status);
        onLoadingData(status);
    }

    public void Client_SetAssetData(string assetdata)
    {
        string jsonData = JsonHelper.fixJson(assetdata);
        assetModel = JsonHelper.FromJson<AssetModel>(jsonData);
        OnAssetData(assetModel);
    }
    public void Client_SetSettlementData(string settlementdata)
    {
        string jsonData = JsonHelper.fixJson(settlementdata);
        SettlementsModel[] settlement_data = JsonHelper.FromJson<SettlementsModel>(jsonData);
        userModel.settlements = settlement_data;
        OnSettlementData(settlement_data);
    }


    public void GetUserMaxNftCount(string max_count)
    {
        // Debug.Log(max_count);
        string jsonData = JsonHelper.fixJson(max_count);
        
        userModel.nft_count = JsonHelper.FromJson<MaxNftDataModel>(jsonData);
        // maxData = JsonHelper.FromJson<MaxNftDataModel>(jsonData);
        // Debug.Log(jsonData);

    }



    public void Client_SetProfessionData(string professiondata)
    {
        string jsonData = JsonHelper.fixJson(professiondata);
        professionData = JsonHelper.FromJson<ProfessionDataModel>(jsonData);
        userModel.professions = professionData;
        OnProfessionData(professionData);
    }

    public void Client_SetItemData(string itemdata)
    {
        string jsonData = JsonHelper.fixJson(itemdata);
        itemData = JsonHelper.FromJson<ItemDataModel>(jsonData);
        userModel.items = itemData;
        OnItemData();
        Debug.Log("Done");
    }

    public void Client_SetInventoryData(string inventory_data)
    {
        string jsonData = JsonHelper.fixJson(inventory_data);
        inventoryData = JsonHelper.FromJson<InventoryModel>(jsonData);
        userModel.inventory = inventoryData;
        // Debug.Log("Set Inventory");
    }

    public void Client_SetBurnInventoryData(string inventory_data)
    {
        string jsonData = JsonHelper.fixJson(inventory_data);
        inventoryData = JsonHelper.FromJson<InventoryModel>(jsonData);
        OnInventoryData(inventoryData);
    }



}