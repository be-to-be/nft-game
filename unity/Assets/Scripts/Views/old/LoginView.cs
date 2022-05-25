using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoginView : BaseView
{
    public GameObject ual_wax;
    public GameObject login_btn;
    public GameObject fetching_data_panel;
    protected override void Start()
    {
        base.Start();
        //MessageHandler.Server_TryAutoLogin();
        MessageHandler.onLoadingData += onLoadingData;
        MessageHandler.OnLoginData += OnLoginData;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageHandler.onLoadingData -= onLoadingData;
        MessageHandler.OnLoginData -= OnLoginData;
    }

    public void OnAnchorButtonClick()
    {
        //MessageHandler.Server_GetUserData("anchor");
    }

    public void OnWaxButtonClick()
    {
        //MessageHandler.Server_GetUserData("cloud");
    }

    public void onCreateButtonClick()
    {
        Application.OpenURL("https://all-access.wax.io/");
    }
    public void OnLoginButtonClick()
    {
        login_btn.SetActive(false);
        ual_wax.SetActive(true);
    }

    private void onLoadingData(string status)
    {
        Debug.Log("In Loading Model " + status);
        if(status == "true")
        {
            ual_wax.SetActive(false);
            fetching_data_panel.SetActive(true);
            login_btn.SetActive(false);
        }
    }

    private void OnLoginData()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
