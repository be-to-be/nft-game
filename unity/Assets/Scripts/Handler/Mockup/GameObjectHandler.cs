using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectHandler : MonoBehaviour
{

    public static void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    public static void TogglePanel(GameObject panel)
    {
        if(panel != null)
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
        }
    }

    public static void ClosePanel(GameObject panel)
    {
        if(panel != null)
        {
            panel.SetActive(false);
        }
    }

    public static void OpenPanel(GameObject panel)
    {
        if(panel != null)
        {
            panel.SetActive(true);
        }
    }
    public void OnLogoutButtonClick()
    {
        MessageHandler.LogoutRequest();
    }

}
