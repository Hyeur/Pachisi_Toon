using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleSettingPannelBtn : BaseButton
{
    protected override void OnClick()
    {
        SessionManager.Instance.toggleSettingPanel();
    }
}
