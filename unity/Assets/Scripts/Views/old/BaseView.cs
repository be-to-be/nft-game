using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;

public class BaseView : MonoBehaviour
{
    //public TMP_Text ErrorText;
    public GameObject LoadingPanel;
    private float time = 0.0f;

    private void Awake()
    {
        //LoadingPanel.SetActive(false);
        //ErrorText = GameObject.Find("ErrorText").GetComponent<TMP_Text>();
        //ErrorText.text = "";
    }

    protected virtual void Start()
    {
        ErrorHandler.OnErrorData += OnErrorData;
    }

    protected virtual void OnDestroy()
    {
        ErrorHandler.OnErrorData -= OnErrorData;
    }

    private void OnErrorData(string errorData)
    {
        //ErrorText.text = errorData;
        //LoadingPanel.SetActive(false);
    }

    private void Update()
    {
        // if (ErrorText.text != "")
        //     time += Time.deltaTime;
        // else
        //     time = 0.0f;

        // if (time > 2.0f)
        //     ErrorText.text = "";
    }

    public void Returnbtn()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

}