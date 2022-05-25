using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    public string url = "";
    public RawImage thisRenderer;
    //public Image nft_img;

    void Start()
    {
        //StartCoroutine(DownloadImage(url)); 
    }

    [System.Obsolete]
    public void loadimg(string url)
    {
        StartCoroutine(DownloadImage(url));
    }

    [System.Obsolete]
    public IEnumerator DownloadImage(string MediaUrl)
    {
        thisRenderer.material.color = Color.white;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            thisRenderer.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        }
        //thisRenderer.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}