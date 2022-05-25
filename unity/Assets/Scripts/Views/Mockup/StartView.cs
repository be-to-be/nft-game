using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartView : BaseView
{
    public GameObject Login_btn;
    public GameObject Start_btn;
    //public GameObject fetching_data_panel;
    protected override void Start()
    {
        base.Start();
        //MessageHandler.Server_TryAutoLogin();
        MessageHandler.onLoadingData += onLoadingData;
        MessageHandler.OnLoginData += doLoginSuccessAction;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.onLoadingData -= onLoadingData;
        MessageHandler.OnLoginData -= doLoginSuccessAction;
    }
    // buttons on PanelLogin
    public void OnAnchorButtonClick()
    {
        MessageHandler.LoginRequest("anchor");
    }
    public void OnWaxButtonClick()
    {
        MessageHandler.LoginRequest("cloud");
        //GameObjectHandler.OpenScene("MapScene");
        
    }
    public void onCreateWalletButtonClick()
    {
        Debug.Log("onCreateWalletButtonClick");
        //Application.OpenURL("https://all-access.wax.io/");
        Application.OpenURL("https://waxel.net/set-up-wax-wallet/");
        
    }
    // public void OnLoginButtonClick()
    // {
    //     login_btn.SetActive(false);
    //     ual_wax.SetActive(true);
    // }

    private void onLoadingData(string status)
    {
        Debug.Log("In Loading Model " + status);
        if(status == "true")
        {
            Login_btn.SetActive(false);
            //LoadingPanel.SetActive(true);
            Start_btn.SetActive(false);
        }
    }

    private void doLoginSuccessAction()
    {
        //SceneManager.LoadScene("MenuScene");
        GameObjectHandler.OpenScene("MapScene");
    }
    //TODO - Adjust to really login, for now just opens the map szene
    public void LoginWaxCloudWallet()
    {
        //GameObjectHandler.OpenScene("MapScene");
    }

    //TODO - Adjust to really login, for now just opens the map szene
    public void LoginAnchorWallet()
    {
        //GameObjectHandler.OpenScene("MapScene");
    }

}
