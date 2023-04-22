using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected GameObject pawnPrefab;
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
        foreach (var team in joinedTeam)
        {
            GameObject[] myPawns = instantiatePrefabs(pawnPrefab,team.transform, pawnAmount);
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
