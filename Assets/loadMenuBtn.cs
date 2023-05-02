using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadMenuBtn : BaseButton
{
    protected override void OnClick()
    {
        SessionManager.Instance.loadScene("MenuScene");
    }
}
