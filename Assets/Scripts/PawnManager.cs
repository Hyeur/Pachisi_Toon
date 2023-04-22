using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;
using Task = System.Threading.Tasks.Task;

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

    public List<Pawn> allPawns;
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
    }
    void Update()
    {
        
    }

    public void loadPawn()
    {
        team1Pawn = _listTeam[0].pawns;
        team2Pawn = _listTeam[1].pawns;
        team3Pawn = _listTeam[2].pawns;
        team4Pawn = _listTeam[3].pawns;
        allPawns.AddRange(team1Pawn);
        allPawns.AddRange(team2Pawn);
        allPawns.AddRange(team3Pawn);
        allPawns.AddRange(team4Pawn);
    }

    public async void resetAllPawn()
    {
        await resetAllPawnByTeam(team1Pawn);
        await resetAllPawnByTeam(team2Pawn);
        await resetAllPawnByTeam(team3Pawn);
        await resetAllPawnByTeam(team4Pawn);

    }
    private async Task resetAllPawnByTeam(List<Pawn> listPawn)
    {
        foreach (Pawn pawn in listPawn)
        {
            await pawn.recoveryToCurrentPad(.5f);
        }
    }

    public async void pawning(Pawn pawn, int step)
    {
        if (!_active) return;
        
        pawn._isMoved = false;

        if (pawn._isOut)
        {
            if (canCageEntry(pawn))
            {
                await checkGatePoint(pawn);
            }
        
        
            if (!pawn._isMoved)
            {
                await movePawnWithStep(pawn,step);
            }
        }
        else
        {
            await checkOut(pawn);
        }
    }
    private async Task movePawnWithStep(Pawn pawn, int step)
    {
        if (PadManager.Instance.anyPawnOnTheWay(pawn, step))
        {
            if (PadManager.Instance.isAttackableEnemyOnTheWay(pawn, step))
            {
                var currentPad = PadManager.Instance.getIndexOfPadByPawn(pawn);
                var targetPawn = PadManager.Instance.getPadAtIndex(currentPad,step).getPawnCaptured();
                kickThePawn(targetPawn);
            }
            else
            {
                cannotMoveandPickAgain(pawn);
                return;
            }
        }
        if (pawn._isFinish || pawn._isMoving && !pawn._isOut && !pawn._isReady && pawn._isMoved)
        {
            cannotMoveandPickAgain(pawn);
            return;
        };
        
        DiceManager.Instance.resetToCenter();
        var tasks = new Task[step];
        for (int i = 0; i < step; i++)
        {
            int currentPadIndex = PadManager.Instance.getIndexOfPadByPawn(pawn);
            Pad destinationPad = PadManager.Instance.getPadAtIndex(currentPadIndex, 1);
            //set new position
            pawn.setCurrentPad(destinationPad);

            //animation
            pawn._isMoving = true;

            tasks[i] = pawn.transform.DOJump(destinationPad.actualPos, 1, 1, movingTimeSpent).AsyncWaitForCompletion();

            await Task.WhenAll(tasks[i]);
            

            
            rotateIfNeed(pawn);
            pawn._isMoving = false;
        }
        pawn._isMoved = true;
        endTurn(pawn._isMoved);
    }

    private async Task checkGatePoint(Pawn pawn)
    {

        Dictionary<int, Pad> GPads;
        switch (pawn.Team.teamName)
        {
            case "Team1":
                GPads = PadManager.Instance.GmapTeam1;
                break;
            case "Team2":
                GPads = PadManager.Instance.GmapTeam2;
                break;
            case "Team3":
                GPads = PadManager.Instance.GmapTeam3;
                break;
            case "Team4":
                GPads = PadManager.Instance.GmapTeam4;
                break;
            default:
                GPads = PadManager.Instance.GmapTeam1;
                break;
        }
        
        int firstEmptyInx = 0;

        if (pawn._isFinish)
        {
            List<int> twoResult = DiceManager.Instance.getTwoResults().OrderBy(x => x).ToList();
            foreach (var item in GPads)
            {
                foreach (int result in twoResult)
                {
                    if (item.Value.isFree && item.Key == result)
                    {
                        var path = GPads.Where(x => x.Key < item.Key && x.Key > GPads.FirstOrDefault(pawnPad => pawnPad.Value == pawn.getCurrentPad()).Key);
                        if (path.All(p => p.Value.isFree == true))
                        {
                            firstEmptyInx = result;
                        }
                    }

                }
            }
            if (firstEmptyInx <= GPads.FirstOrDefault(i => i.Value == pawn.getCurrentPad()).Key)
            {
                cannotMoveandPickAgain(pawn);
                return;
            }
        }
        else
        {
            List<int> twoResult = DiceManager.Instance.getTwoResults().OrderBy(x => x).ToList();
            foreach (var item in GPads)
            {
                foreach (int result in twoResult)
                {
                    if (item.Value.isFree && item.Key == result)
                    {
                        var path = GPads.Where(x => x.Key < item.Key && x.Key > GPads.FirstOrDefault(pawnPad => pawnPad.Value == pawn.getCurrentPad()).Key);
                        if (path.All(p => p.Value.isFree == true))
                        {
                            firstEmptyInx = result;
                        }
                    }

                }
            }
        }
        
        if (!pawn._isMoving && pawn._isOut && pawn._isReady && firstEmptyInx > 0 && !pawn._isMoved)
        {
            
            DiceManager.Instance.resetToCenter();
            Pad destinationPad = GPads[firstEmptyInx];
            pawn.setCurrentPad(destinationPad);
            pawn._isMoving = true;
            await pawn.transform.DOJump(destinationPad.actualPos, 3, 1, 1.5f).SetEase(Ease.InSine).OnComplete(() =>
            {
                rotateIfNeed(pawn);
                pawn._isMoving = false;
                pawn._isMoved = true;
                pawn._isFinish = true;
                endTurn(pawn._isMoved);
            }).AsyncWaitForCompletion();
            
        }
    }

    private async Task checkOut(Pawn pawn)
    {
        var _twoResult = DiceManager.Instance.getTwoResults();
        if (_twoResult.Any(result => result.Equals(1) || result.Equals(6)))
        {
             moveToSpawn(pawn);
             rotateIfNeed(pawn);
             endTurn(pawn._isMoved);
        }
        else
        {
            cannotMoveandPickAgain(pawn);
        }
    }

    public void rotateIfNeed(Pawn pawn)
    {
        if (pawn._isReady)
        {
            var currentPadIndx = PadManager.Instance.getIndexOfPad(pawn.getCurrentPad());
            pawn.transform.DOLookAt(2 * pawn.transform.position - PadManager.Instance.getPadAtIndex(currentPadIndx,1).transform.position, .1f);
        }
    }

    private void cannotMoveandPickAgain(Pawn pawn)
    {
        Debug.Log($"can not move {pawn}");
        pawn.transform.DOShakeScale(0.1f, 0.1f, 1);
        GameManager.Instance.updateGameState(GameManager.GameState.PickAPawn);
    }

    private bool canCageEntry(Pawn pawn)
    {
        if ((pawn.getCurrentPad().isGate || pawn._isFinish) && pawn.getCurrentPad().padTeam.Contains(pawn.Team.teamName))
        {
            return true;
        }
        
        return false;
    }

    public List<Pawn> getAllPawns()
    {
        return allPawns;
    }
    private void endTurn(bool isMoved)
    {
        if (_active && isMoved)
        {
            Debug.Log("Pawning Ended");
            GameManager.Instance.updateGameState(GameManager.GameState.SwitchTeam);
        }
    }

    private void kickThePawn(Pawn kickedPawn)
    {
        kickedPawn.setCurrentPad(null);
        kickedPawn.sentToHome(1f);
    }

    private void moveToSpawn(Pawn pawn)
    {
        var startPad = PadManager.Instance.getSpawnPadByTeam(pawn.Team);
        if (startPad)
        {
            pawn.setCurrentPad(startPad);
            pawn.recoveryToCurrentPad(.5f);
            pawn._isMoved = true;
        }
    }
}
