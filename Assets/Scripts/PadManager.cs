using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

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

    public Dictionary<int, Pad> map = new Dictionary<int, Pad>();
    void Start()
    {
        Instance = this;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createMainPath(List<Pad> listAllPads)
    {
        for (int i = 0; i < listAllPads.Count; i++)
        {
            map.Add(i+1, listAllPads[i]);
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
            team.Value[0] = team.Value[0].OrderBy(o => int.Parse(o.padName.Trim('L'))).ToList();
            team.Value[1] = team.Value[1].OrderBy(o => int.Parse(o.padName.Trim('G'))).ToList();
            _mainPath.AddRange(team.Value[0]);
        }

        if (_mainPath.Count > 0)
        {
            createMainPath(_mainPath);
        }
    }
}
