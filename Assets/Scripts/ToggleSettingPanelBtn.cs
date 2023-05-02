using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSettingPanelBtn : BaseButton
{
    protected override void OnClick()
    {
        SessionManager.Instance.toggleSettingPanel();
    }
}
