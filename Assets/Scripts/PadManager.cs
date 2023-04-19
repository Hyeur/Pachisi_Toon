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
    
    [SerializeField] protected List<Pad> team2LPads;
                                      
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
        listGPads = listGPads.OrderBy(o => int.Parse(o.padName.Trim('G'))).ToList();
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
            {t2,new List<List<Pad>>(){team2LPads,team3GPads}},
            {t3,new List<List<Pad>>(){team3LPads,team2GPads}},
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
            team.Value[0] = team.Value[0].OrderBy(o => int.Parse(o.padName.Trim('L'))).ToList();
            team.Value[1] = team.Value[1].OrderBy(o => int.Parse(o.padName.Trim('G'))).ToList();
            
            
            _mainPath.AddRange(team.Value[0]);
        }

        if (_mainPath.Count > 0)
        {
            createMainPath(_mainPath);
        }        
        loadGmap();
    }

    public int getCurrentPadIndexofPawn(Pawn pawn)
    {
        return Lmap.FirstOrDefault(key => key.Value == pawn.getCurrentPad()).Key;
    }
    public int getTheNextPadIndxOfPawn(Pawn pawn)
    {
        int inx = getCurrentPadIndexofPawn(pawn) + 1;
        if (inx > 40)
        {
            inx -= 40;
        }

        return inx;
    }


    public bool anyPawnOnTheWay(Pawn pawn, int step)
    {
        // int[] path ;
        // for (int i = 0; i < step; i++)
        // {
        //     Debug.Log(getTheNextPadIndxOfPawn(pawn) + i);
        //     path[i] = getTheNextPadIndxOfPawn(pawn) + i;
        // }

       // return path.Any(pad => Lmap[pad].isFree == false);
       return Lmap.Keys.Any(padIndex => Lmap[padIndex].isFree == false && padIndex > getCurrentPadIndexofPawn(pawn) &&
                                        padIndex <= getCurrentPadIndexofPawn(pawn) + step);
    }
    
}
