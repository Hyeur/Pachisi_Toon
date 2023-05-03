using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSetUpManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI playerCountTxt;
    [SerializeField] protected int maxPlayer = 4;

    private void Start()
    {
        playerCountTxt.text = "2";
    }

    public void addPlayer()
    {
        if (Int32.Parse(playerCountTxt.text) == maxPlayer) return;

        playerCountTxt.text = (Int32.Parse(playerCountTxt.text) + 1).ToString();
        SessionManager.Instance.playerCount = playerCountTxt.text;
    }

    public void popPlayer()
    {
        if (Int32.Parse(playerCountTxt.text) == 2) return;
        playerCountTxt.text = (Int32.Parse(playerCountTxt.text) - 1).ToString();
        SessionManager.Instance.playerCount = playerCountTxt.text;
    }
    
}
