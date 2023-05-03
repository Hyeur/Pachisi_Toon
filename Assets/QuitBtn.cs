using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitBtn : BaseButton
{
    protected override void OnClick()
    {
        Application.Quit();
    }
}
