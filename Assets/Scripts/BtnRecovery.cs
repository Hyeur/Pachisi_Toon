using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BtnRecovery : BaseButton
{
    protected override void OnClick()
    {
        PawnManager.Instance.resetAllPawn();
        Debug.Log("resetAllPawn");
    }
}
