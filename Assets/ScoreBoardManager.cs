using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    [SerializeField] protected List<Image> rankingList;

    private void Start()
    {
        if (GameManager.Instance.rankedList.Count < 1)
        {
            Debug.Log("no ranking");
            return;
        }
        else
        {
            foreach (var team in GameManager.Instance.rankedList)
            {
                Debug.Log(team.teamName);
            }
            for (int i = 0; i < GameManager.Instance.rankedList.Count; i++)
            {
                rankingList[i].sprite = HUDManager.Instance._teamAvatar[GameManager.Instance.rankedList[i]];
                rankingList[i].color = Color.white;
            }
        }
    }
}
