using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected GameObject team1pawnPrefab;
    [SerializeField] protected GameObject team2pawnPrefab;
    [SerializeField] protected GameObject team3pawnPrefab;
    [SerializeField] protected GameObject team4pawnPrefab;
    public List<Team> playTeam;
    void Awake()
    {
        Debug.Log(TeamManager.Instance.TEAMS.Count);
        foreach (var stringTeam in SessionManager.Instance.teams)
        {
            Team tempTeam = TeamManager.Instance.TEAMS.FirstOrDefault(t => t.teamName.Equals(stringTeam));
            if (tempTeam)
            {
                playTeam.Add(tempTeam);
            }
        }
    }

    private void Start()
    {
        if (playTeam.Count > 0)
        {
            initPawnsForTeams(playTeam, 4);
        }
    }

    void Update()
    {
        
    }

    public void initPawnsForTeams(List<Team> joinedTeam,int pawnAmount)
    {
        GameObject pawnPrefab;
        foreach (var team in joinedTeam)
        {
            switch (team.teamName)
            {
                case "Team1":
                    pawnPrefab = team1pawnPrefab;
                    break;
                case "Team2":
                    pawnPrefab = team2pawnPrefab;
                    break;
                case "Team3":
                    pawnPrefab = team3pawnPrefab;
                    break;
                case "Team4":
                    pawnPrefab = team4pawnPrefab;
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
