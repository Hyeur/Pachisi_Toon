using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMoveTest : BaseButton
{
    public Pawn pawn;
    public int step = 3;
    protected override void OnClick()
    {
        int diceResult = (int)DiceManager.Instance.getTotalResult();
        PawnManager.Instance.movePawn(pawn,diceResult);
    }
}
