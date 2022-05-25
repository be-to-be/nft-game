//import axios from 'axios';

const wax = new waxjs.WaxJS({
  rpcEndpoint: 'https://waxtestnet.greymass.com',
  tryAutoLogin: false
});

var loggedIn = false;
var anchorAuth = "owner";

const dapp = "WaxelNinjas";
const endpoint = "testnet.wax.pink.gg";
const contract = "waxelworld11";
const tokenContract = 'waxeltokens1';
const collectionName = 'laxewneftyyy';
const schemaName = 'laxewnefty';

  async function autoLogin(){
    /*var isAutoLoginAvailable = await wallet_isAutoLoginAvailable();
    if (isAutoLoginAvailable) {
      login();
    } */
  }

  async function wallet_isAutoLoginAvailable() {
    const transport = new AnchorLinkBrowserTransport();
    const anchorLink = new AnchorLink({
      transport,
      chains: [{
        chainId: 'f16b1833c747c43682f4386fca9cbb327929334a762755ebec17f6f23c9b8a12',
        nodeUrl: 'https://waxtestnet.greymass.com',
      }],
    });
    var sessionList = await anchorLink.listSessions(dapp);
    if (sessionList && sessionList.length > 0) {
      useAnchor = true;
      return true;
    } else {
      useAnchor = false;
      return await wax.isAutoLoginAvailable();
    }
  }

  async function selectWallet(walletType) {
    wallet_selectWallet(walletType);
    login();
  }

  async function wallet_selectWallet(walletType) {
    useAnchor = walletType == "anchor";
  }

  async function login() {
    try {
        const userAccount = await wallet_login();
        sendUserData();
    } catch (e) {
      unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e.message);
    }
  }

  async function wallet_login() {
    const transport = new AnchorLinkBrowserTransport();
    const anchorLink = new AnchorLink({
      transport,
      chains: [{
        chainId: 'f16b1833c747c43682f4386fca9cbb327929334a762755ebec17f6f23c9b8a12',
        nodeUrl: 'https://waxtestnet.greymass.com',
      }],
    });
    if (useAnchor) {
      var sessionList = await anchorLink.listSessions(dapp);
      if (sessionList && sessionList.length > 0) {
        wallet_session = await anchorLink.restoreSession(dapp);
      } else {
        wallet_session = (await anchorLink.login(dapp)).session;
      }
      wallet_userAccount = String(wallet_session.auth).split("@")[0];
      auth = String(wallet_session.auth).split("@")[1];
      anchorAuth = auth;
    } else {
      wallet_userAccount = await wax.login();
      wallet_session = wax.api;
      anchorAuth = "active";
    }
    return wallet_userAccount;
  }

    async function sendUserData(){
      try {

          let assets = getAssets();
          assetData = await assets;

          let user = getUserData();
          userData = await user;

          let ninja = getNinjaData();
          ninjaData = await ninja;

          let profession = getProfessionData();
          professionData = await profession;

          let item = getItemsData();
          itemData = await item;

          let obj = {
            account:userAccount.toString(),
            userdata: userData,
            ninjas:ninjaData,
            professions:professionData,
            items:itemData,
          }

          console.log(obj);

          unityInstance.SendMessage(
            "GameController",
            "Client_SetUserData",
            obj === undefined ? JSON.stringify({}) : JSON.stringify(obj)
          );
      }
      catch (e) {
        unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e.message);
      }
    }


      async function getAssets(schema) {
        var path = "atomicassets/v1/assets?collection_name=" + collectionName + "&schema_name=" + schema +  "&owner=" + userAccount + "&page=1&limit=1000&order=desc&sort=asset_id";
        try 
        {
            const response = await fetch("https://" + "test.wax.api.atomicassets.io/" + path, {
            headers: {
              "Content-Type": "text/plain"
            },
            method: "POST",
          });
        
          const body = await response.json();
          return body.data;
        }
        catch (e){
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e);
        }
      }

      async function getUserData(){
        var path = "/v1/chain/get_table_rows";

        var data = JSON.stringify({
          json: true,
          code: contract,
          scope: contract,
          table: "user",
          limit: 1,
          lower_bound: userAccount,
          upper_bound: userAccount,
        });

        const response = await fetch("https://" + endpoint + path, {
          headers: {
            "Content-Type": "text/plain"
          },
          body: data,
          method: "POST",
        });

        const body = await response.json();
        if(body.rows.length != 0)
          return body.rows;
        else return 0;
      }

       async function getAssetD(){
        try {
            
          let assets = getAssets();
          assetdata = await assets;

          console.log(assetdata);
          
          unityInstance.SendMessage(
            "GameController",
            "Client_SetAssetData",
            assetdata === undefined ? JSON.stringify({}) : JSON.stringify(assetdata)
          );
        }
        catch (e){
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e);
        }
      }

      async function getNinjaData() {
        try {
          var path = "/v1/chain/get_table_rows";

          var data = JSON.stringify({
            json: true,
            code: contract,
            scope: contract,
            table: "ninjas",
            key_type: "name",
            index_position: 2,
            lower_bound: userAccount,
            upper_bound: userAccount,
            limit: 1000,
          });

          const response = await fetch("https://" + endpoint + path, {
            headers: {
              "Content-Type": "text/plain"
            },
            body: data,
            method: "POST",
          });

          const body = await response.json();
          let assets = getAssets("citizens");
          assetData = await assets;

          var ninja_data = [];
          for(i=0; i< body.rows.length; i++){
            data = body.rows[i];
            for(j=0; j < assetData.length; j++){
              if(data.asset_id == assetData[j].asset_id && data.owner == assetData[j].owner)
                ninja_data.push({
                  asset_id:data.asset_id,
                  delay_seconds:data.delay_seconds,
                  last_search:data.last_search,
                  race:data.race,
                  img:assetData[j].img,
                  reg:data.asset_id == (assetData[j].asset_id && data.owner == assetData[j].owner) ? "1":"0",
                });
            }
          }
          console.log(ninja_data);
          return ninja_data;
        }
        catch (e) {
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e);
        }
      }

      async function getProfessionData() {
        try {
          var path = "/v1/chain/get_table_rows";

          var data = JSON.stringify({
            json: true,
            code: contract,
            scope: contract,
            table: "professions",
            key_type: "uint64",
            index_position: 2,
            //lower_bound: userAccount,
            //upper_bound: userAccount,
            limit: 1000,
          });

          const response = await fetch("https://" + endpoint + path, {
            headers: {
              "Content-Type": "text/plain"
            },
            body: data,
            method: "POST",
          });

          const body = await response.json();
          let assets = getAssets("professions");
          assetData = await assets;

          var profession_data = [];
          var unreg_profession_data = [];
          for(i=0; i< body.rows.length; i++){
            data = body.rows[i];
            for(j=0; j < assetData.length; j++){
              if(data.asset_id == assetData[j].asset_id && data.owner == assetData[j].owner)
                profession_data.push({
                  asset_id:data.asset_id,
                  type:data.type,
                  name:data.name,
                  lastsearch:data.last_material_search,
                  usesleft:data.uses_left,
                  status:data.status,
                  img:assetData[j].img,
                  reg:data.asset_id == (assetData[j].asset_id && data.owner == assetData[j].owner) ? "1":"0",  //1 = Reg && 0 = UnReg
                });
            }
          }
            
          return {profession_data , unreg_profession_data};
        }
        catch (e) {
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e);
        }
      }

      async function getItemsData() {
        try {
          var path = "/v1/chain/get_table_rows";

          var data = JSON.stringify({
            json: true,
            code: contract,
            scope: contract,
            table: "items",
            key_type: "name",
            index_position: 2,
            lower_bound: userAccount,
            upper_bound: userAccount,
            limit: 1000,
          });

          const response = await fetch("https://" + endpoint + path, {
            headers: {
              "Content-Type": "text/plain"
            },
            body: data,
            method: "POST",
          });

          const body = await response.json();
          let assets = getAssets("items");
          assetData = await assets;

          var items_data = [];
          for(i=0; i< body.rows.length; i++){
            data = body.rows[i];
            for(j=0; j < assetData.length; j++){
              if(data.asset_id == assetData[j].asset_id && data.owner == assetData[j].owner)
                items_data.push({
                  asset_id:data.asset_id,
                  name:data.name,
                  lastsearch:data.last_material_search,
                  usesleft:data.uses_left,
                  status:data.status,
                  img:assetData[j].img,
                  function:data.function
                });
            }
          }
            
          return items_data;
        }
        catch (e) {
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e);
        }
      }

      async function getNinjaD(){
        try {
          let ninjas = getNinjaData();
          ninjadata = await ninjas;
          
          unityInstance.SendMessage(
            "GameController",
            "Client_SetNinjaData",
            ninjadata === undefined ? JSON.stringify({}) : JSON.stringify(ninjadata)
          );
        }
        catch (e){
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", "unable to fetch ninja data");
        }
      }

      async function search_citizen(assetid){
        try {
          console.log(assetid);
          await anchorSession.transact({
            action: {
              account: 'waxelnftgame',
              name: 'searchforcz',
              authorization: [anchorSession.auth],
              data: {
                account: anchorSession.auth.actor,
                ninjaID: assetid,
              }
            },
          })
        } 
        catch(e) {
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e.message);
        }
      }

      async function start_search(assetid){
        try{

        }
        catch(e){
          
        }
      }

      async function registerAsset(assetid){
        try {
          console.log(assetid);
          await anchorSession.transact({
            action: {
              account: 'waxelnftgame',
              name: 'registernft',
              authorization: [anchorSession.auth],
              data: {
                asset_id: assetid,
                owner: anchorSession.auth.actor,
              }
            },
          })
        } 
        catch(e) {
          unityInstance.SendMessage("ErrorHandler", "Client_SetErrorData", e.message);
        }
      }
