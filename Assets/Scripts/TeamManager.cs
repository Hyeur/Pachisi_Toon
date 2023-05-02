using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;

    public List<Team> TEAMS;
    private void Awake()
    {
        Instance = this;
        TEAMS = GetComponentsInChildren<Team>().ToList();
        GameManager.OnBeforeGameStateChanged += beforeGameManagerOnBeforeGameStateChanged;
    }

    private void beforeGameManagerOnBeforeGameStateChanged(GameManager.GameState state)
    {
        
    }

    private void OnDestroy()
    {
        GameManager.OnBeforeGameStateChanged -= beforeGameManagerOnBeforeGameStateChanged;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Team> getUnFinishTeams()
    {
        return TEAMS.Where(t => t.getNumsFinishPawn() < t.getNumsPawn() && t.getNumsPawn() > 0).ToList();
    }

    public void checkWinCondition(Team team)
    {
        if (team.pawns.All(p => p.isFinish == true))
        {
            team.isWin = true;
            GameManager.Instance.rankedList.Add(team);
        }
    }
}
