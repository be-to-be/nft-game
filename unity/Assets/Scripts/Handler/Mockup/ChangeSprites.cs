using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprites : MonoBehaviour
{
    public Sprite[] sprites;
    public Image targetImage;

    public void ChangeSprite()
    {
        if (targetImage.sprite == sprites[0])
        {
            targetImage.sprite = sprites[1];
            return;
        }
        targetImage.sprite = sprites[0];
    }
}
