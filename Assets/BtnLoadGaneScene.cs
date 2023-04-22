using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnLoadGaneScene : BaseButton
{
    protected override void OnClick()
    {
        SessionManager.Instance.LoadScene("GameScene");
    }
}
