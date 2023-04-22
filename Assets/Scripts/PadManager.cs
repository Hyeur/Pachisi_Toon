using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PadManager : MonoBehaviour
{
    public static PadManager Instance;
    
    [SerializeField] protected GameObject t1;
    [SerializeField] protected GameObject t2;
    [SerializeField] protected GameObject t3;
    [SerializeField] protected GameObject t4;
    
    [SerializeField] protected List<Pad> team1LPads;
    
    [SerializeField] private List<Pad> team2LPads;
                                      
    [SerializeField] protected List<Pad> team3LPads; 
                                      
    [SerializeField] protected List<Pad> team4LPads;
    
    [SerializeField] protected List<Pad> team1GPads;
    
    [SerializeField] protected List<Pad> team2GPads;
                                      
    [SerializeField] protected List<Pad> team3GPads;
                                      
    [SerializeField] protected List<Pad> team4GPads;

    [SerializeField] protected List<Pad> _mainPath;

    public Dictionary<int, Pad> Lmap = new Dictionary<int, Pad>();
    public Dictionary<int, Pad> GmapTeam1 = new Dictionary<int, Pad>();
    public Dictionary<int, Pad> GmapTeam2 = new Dictionary<int, Pad>();
    public Dictionary<int, Pad> GmapTeam3 = new Dictionary<int, Pad>();
    public Dictionary<int, Pad> GmapTeam4 = new Dictionary<int, Pad>();
    private void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        loadPads();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void createMainPath(List<Pad> listLPads)
    {
        for (int i = 0; i < listLPads.Count; i++)
        {
            Lmap.Add(i+1, listLPads[i]);
        }
    }

    private void loadGmap()
    {
        createGoalPath(GmapTeam1,team1GPads);
        createGoalPath(GmapTeam2,team2GPads);
        createGoalPath(GmapTeam3,team3GPads);
        createGoalPath(GmapTeam4,team4GPads);
    }
    private void createGoalPath(Dictionary<int, Pad> gmap, List<Pad> listGPads)
    {
        for (int i = 0; i < listGPads.Count; i++)
        {
            gmap.Add(i+1, listGPads[i]);
        }
    }

    public void loadPads()
    {
        var TEAMS = new Dictionary<GameObject,List<List<Pad>>>()
        {
            {t1,new List<List<Pad>>(){team1LPads,team1GPads}},
            {t2,new List<List<Pad>>(){team2LPads,team2GPads}},
            {t3,new List<List<Pad>>(){team3LPads,team3GPads}},
            {t4,new List<List<Pad>>(){team4LPads,team4GPads}}
        };

        foreach (var team in TEAMS)
        {
            if (team.Value[0].Count > 0 || team.Value[1].Count > 0)
                return;
            foreach (Pad pad in team.Key.GetComponentsInChildren<Pad>())
            {
                //add Tag
                if (!pad.gameObject.CompareTag("Pad"))
                {
                    pad.gameObject.tag = "Pad";
                }

                if (pad.padName.Contains("L"))
                {
                    if (pad.padName.Contains("0"))
                    {
                        pad.isGate = true;
                    }
                    team.Value[0].Add(pad);
                }

                if (pad.padName.Contains("G"))
                {
                    team.Value[1].Add(pad);
                }
            } 
        }
        
        team1LPads = team1LPads.OrderBy(o => int.Parse(o.padName.Trim('L'))).ToList();
        team2LPads = team2LPads.OrderBy(o => int.Parse(o.padName.Trim('L'))).ToList();
        team3LPads = team3LPads.OrderBy(o => int.Parse(o.padName.Trim('L'))).ToList();
        team4LPads = team4LPads.OrderBy(o => int.Parse(o.padName.Trim('L'))).ToList();
        
        team1GPads = team1GPads.OrderBy(o => int.Parse(o.padName.Trim('G'))).ToList();
        team2GPads = team2GPads.OrderBy(o => int.Parse(o.padName.Trim('G'))).ToList();
        team3GPads = team3GPads.OrderBy(o => int.Parse(o.padName.Trim('G'))).ToList();
        team4GPads = team4GPads.OrderBy(o => int.Parse(o.padName.Trim('G'))).ToList();
        
        _mainPath.AddRange(team1LPads);
        _mainPath.AddRange(team3LPads);
        _mainPath.AddRange(team2LPads);
        _mainPath.AddRange(team4LPads);

        if (_mainPath.Count > 0)
        {
            createMainPath(_mainPath);
        }        
        loadGmap();
    }
    
    // public int getTheNextPadIndxOfPawn(Pawn pawn)
    // {
    //     
    // }

    public Pad getPadAtIndex(int index, int additionalIndex = 0)
    {
        index = remapTheIndx(index + additionalIndex);
        return Lmap[index];
    }

    public int getIndexOfPad(Pad pad)
    {
        return Lmap.FirstOrDefault(key => key.Value == pad).Key;
    }
    
    public int getIndexOfPadByPawn(Pawn pawn)
    {
        return Lmap.FirstOrDefault(key => key.Value == pawn.getCurrentPad()).Key;
    }

    private int remapTheIndx(int _base, int _add = 0)
    {
        int result = _base + _add;
        if (result > 40)
        {
            result -= 40;
        }
        return result;
    }


    public bool anyPawnOnTheWay(Pawn pawn, int step)
    {
        Pad[] path = new Pad[step];
        for (int i = 0; i < step; i++)
        {
            path[i] = (getPadAtIndex(getIndexOfPad(pawn.getCurrentPad()) + i + 1));
        }

        return path.Any(pad => pad.isFree == false);
    }

    public bool isAttackableEnemyOnTheWay(Pawn pawn, int step)
    {
        var isLastPadOnTheWayFree = getPadAtIndex(getIndexOfPad(pawn.getCurrentPad()), step).isFree;
        if (!isLastPadOnTheWayFree)
        {
            Pawn targetPawn = getPadAtIndex(getIndexOfPad(pawn.getCurrentPad()), step).getPawnCaptured();
            var isEnemy = targetPawn.Team != pawn.Team;
            if (anyPawnOnTheWay(pawn, step - 1)) return false;
            if (isEnemy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public Pad getSpawnPadByTeam(Team team)
    {
        switch (team.teamName)
        {
            case "Team1":
                return team1LPads[1];
            case "Team2":
                return team2LPads[1];
            case "Team3":
                return team3LPads[1];
            case "Team4":
                return team4LPads[1];
            default:
                Debug.Log("Cant find the start pad");
                return Lmap[0];
        }
    }
}
