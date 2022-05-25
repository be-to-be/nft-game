using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : BaseView
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void BuyButton(string url)
    {
        Application.OpenURL(url);
    }

}
