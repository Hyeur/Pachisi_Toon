using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenBtn : BaseButton
{
    public Image image;
    public Sprite checkedImage;
    public Sprite uncheckedImage;
    protected override void OnClick()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (Screen.fullScreen)
        {
            image.sprite = checkedImage;
        }
        else
        {
            image.sprite = uncheckedImage;
        }
    }
}
