using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PawnManager : MonoBehaviour
{
    public static PawnManager Instance;
    
    [SerializeField] protected float movingTimeSpent = 0.3f;
    
    [SerializeField] protected Team t1;
    [SerializeField] protected Team t2;
    [SerializeField] protected Team t3;
    [SerializeField] protected Team t4;
    
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
        var teams = new Dictionary<Team,List<Pawn>>()
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
            if (!pawn._isReady)
            {
                pawn.recoveryToCurrentPad();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void movePawn(Pawn pawn, int step)
    {
        StartCoroutine(movePawnOneStep(pawn,step));
    }

    private IEnumerator movePawnOneStep(Pawn pawn, int stack)
    {
        for (int i = 0; i < stack; i++)
        {
            if (!pawn._isFinish && !pawn._isMoving && pawn._isOut && pawn._isReady )
            {
                int destinationIndex = pawn.getCurrentPadIndex() + 1;

                if (destinationIndex > 40)
                {
                    destinationIndex -= 40;
                }
                Pad destinationPad = PadManager.Instance.map[destinationIndex];
                //set new position
                pawn.setCurrentPad(destinationPad);
                //animation
                pawn.getRb().freezeRotation = true;
                pawn._isMoving = true;
                pawn.transform.DOJump(destinationPad.actualPos,1,1,movingTimeSpent);
                yield return new WaitForSeconds(movingTimeSpent);
                pawn._isMoving = false;
                pawn.getRb().freezeRotation = false;
            }
        }
    }
    
}
