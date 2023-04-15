using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PawnManager : MonoBehaviour
{
    public static PawnManager Instance;

    private bool _active;
    
    [SerializeField] protected float movingTimeSpent = 0.3f;
    
    [SerializeField] protected List<Team> _listTeam;

    [SerializeField] protected List<Pawn> team1Pawn;
    
    [SerializeField] protected List<Pawn> team2Pawn;
                                      
    [SerializeField] protected List<Pawn> team3Pawn; 
                                      
    [SerializeField] protected List<Pawn> team4Pawn;
    
    private void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        _active = (state == GameManager.GameState.Pawning);
    }
    
    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void Start()
    {

        _listTeam = TeamManager.Instance.TEAMS;
        
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
            {_listTeam[0],team1Pawn},
            {_listTeam[1],team2Pawn},
            {_listTeam[2],team3Pawn},
            {_listTeam[3],team4Pawn}
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
            team.Key.pawns = team.Value;
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
        yield return new WaitForSeconds(.5f);
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
        if (!_active) return;
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
                
                
                if (stack == i + 1)
                {
                    GameManager.Instance.updateGameState(GameManager.GameState.SwitchTeam);
                }
            }

            
        }
    }
    
}
