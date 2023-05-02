using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    
    [Header("Team1 Asset")]
    [SerializeField] protected GameObject team1pawnPrefab;

    public Sprite team1Avatar;
    
    [Header("Team2 Asset")]
    [SerializeField] protected GameObject team2pawnPrefab;
    public Sprite team2Avatar;
    
    [Header("Team3 Asset")]
    [SerializeField] protected GameObject team3pawnPrefab;
    public Sprite team3Avatar;
    
    [Header("Team4 Asset")]
    [SerializeField] protected GameObject team4pawnPrefab;
    public Sprite team4Avatar;

    public List<Team> playTeam;
    
    private Dictionary<Team, Sprite> _teamCatalouge;
    void Awake()
    {
        Spawner.Instance = this;

#if UNITY_EDITOR
        if ( !SessionManager.Instance || SessionManager.Instance.teams == null)
        {
            Debug.LogWarning("Session team null! \n add team for debug");
            foreach (Team team in TeamManager.Instance.TEAMS)
            {
                this.playTeam.Add(team);
            }
            if (this.playTeam.Count > 0)
            {
                initPawnsForTeams(this.playTeam, 4);
            }
            return;
        }
#endif
        foreach (var stringTeam in SessionManager.Instance.teams)
        {
            Team tempTeam = TeamManager.Instance.TEAMS.FirstOrDefault(t => t.teamName.Equals(stringTeam));
            if (tempTeam)
            {
                playTeam.Add(tempTeam);
            }
        }
        if (this.playTeam.Count > 0)
        {
            initPawnsForTeams(this.playTeam, 4);
        }

    }
    
    public void initPawnsForTeams(List<Team> joinedTeam,int pawnAmount)
    {
        GameObject pawnPrefab;
        foreach (var team in joinedTeam)
        {
            switch (team.teamName)
            {
                case "Team1":
                    pawnPrefab = this.team1pawnPrefab;
                    break;
                case "Team2":
                    pawnPrefab = this.team2pawnPrefab;
                    break;
                case "Team3":
                    pawnPrefab = this.team3pawnPrefab;
                    break;
                case "Team4":
                    pawnPrefab = this.team4pawnPrefab;
                    break;
                default:
                    pawnPrefab = null;
                    Debug.Log("Cant fetch prefab of " + team.teamName);
                    break;
            }

            if (pawnPrefab)
            {
                GameObject[] myPawns = instantiatePrefabs(pawnPrefab,team.transform, pawnAmount);
            }
        }
    }

    private GameObject[] instantiatePrefabs(GameObject prefab,Transform team, int amount)
    {
        GameObject[] result = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            result[i] = GameObject.Instantiate(prefab,team.position,Quaternion.identity,team);
        }
        return result;
    }
    
}
