using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour
{
    public static PawnManager Instance;
    
    [SerializeField] protected GameObject t1;
    [SerializeField] protected GameObject t2;
    [SerializeField] protected GameObject t3;
    [SerializeField] protected GameObject t4;
    
    [SerializeField] protected List<Pawn> team1Pawn;
    
    [SerializeField] protected List<Pawn> team2Pawn;
                                      
    [SerializeField] protected List<Pawn> team3Pawn; 
                                      
    [SerializeField] protected List<Pawn> team4Pawn;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        loadPawn();
        //resetAllPawn();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadPawn()
    {
        var teams = new Dictionary<GameObject,List<Pawn>>()
        {
            {t1,team1Pawn},
            {t2,team2Pawn},
            {t3,team3Pawn},
            {t4,team4Pawn}
        };

        foreach (var team in teams)
        {
            if (team.Key.transform.childCount > 0)
            {
                foreach (Pawn pawn in team.Key.GetComponentsInChildren<Pawn>())
                {
                    pawn.Team = team.Key;
                    team.Value.Add(pawn);
                }
            }
        }
    }

    public void resetAllPawn()
    {
        StartCoroutine(resetAllPawnByTeam(team1Pawn));
        StartCoroutine(resetAllPawnByTeam(team2Pawn));
        StartCoroutine(resetAllPawnByTeam(team3Pawn));
        StartCoroutine(resetAllPawnByTeam(team4Pawn));

    }
    private IEnumerator resetAllPawnByTeam(List<Pawn> listPawn)
    {
        yield return new WaitForSeconds(1f);
        foreach (Pawn pawn in listPawn)
        {
            pawn.respawn();
            yield return new WaitForSeconds(1f);
        }
    }

    public void movePawn(Pawn pawn, int step)
    {
        if (!pawn._isFinish && pawn._isOut && pawn._isReady)
        {
            
            int destinationIndex = pawn.getCurrentPadIndex() + step;

            if (destinationIndex > 40)
            {
                destinationIndex -= 40;
            }
            Pad destinationPad = PadManager.Instance.map[destinationIndex];
            //set new position
            pawn.setCurrentPad(destinationPad);
            
            //animation
            pawn.transform.position = destinationPad.actualPos;
        }
    }
    
}
