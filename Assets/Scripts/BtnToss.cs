using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnToss : BaseButton
{
    protected override void OnClick()
    {
        DiceManager.Instance.toss(GameManager.Instance.currentTeam);
    }
}
