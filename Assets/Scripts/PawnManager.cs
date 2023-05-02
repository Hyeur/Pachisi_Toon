using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
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
    private void beforeGameManagerOnBeforeGameStateChanged(GameManager.GameState state)
    {
        _active = (state == GameManager.GameState.Pawning || state == GameManager.GameState.PickAPawn);
    }
    
    private void Awake()
    {
        Instance = this;
        GameManager.OnBeforeGameStateChanged += beforeGameManagerOnBeforeGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnBeforeGameStateChanged -= beforeGameManagerOnBeforeGameStateChanged;
    }

    void Start()
    {

        _listTeam = TeamManager.Instance.TEAMS;

        if (_listTeam.Count > 0)
        {
            loadPawn();
        }
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

    public async void pawning(Pawn pawn, int step, bool forPrediction = false)
    {
        if (!_active) return;
        
        pawn.isMoved = false;
        

        if (pawn.isOut)
        {
            if (forPrediction) //check for Prediction?
            {
                if (canCageEntry(pawn))
                {
                    await checkGatePoint(pawn,true);
                }
            }
            else
            {
                if (canCageEntry(pawn))
                {
                    await checkGatePoint(pawn);
                }
            }

            if (forPrediction && pawn.isCanNotMoveForPrediction) //check for Prediction?
            {
                return;
            }
        
        
            if (!pawn.isMoved) //check for Prediction?
            {
                if (forPrediction)
                {
                    await movePawnWithStep(pawn,step,true);
                }
                else
                {
                    await movePawnWithStep(pawn,step);
                }
            }
        }
        else
        {
            await checkOut(pawn,forPrediction);
        }

        // if (pawn.isMoved)
        // {
        //     if (DiceManager.Instance.allOneOrSix())
        //     {
        //         GameManager.Instance.updateGameState(GameManager.GameState.RollTheDice);
        //     }
        // }
    }
    private async Task movePawnWithStep(Pawn pawn, int step, bool forPrediction = false)
    {
        if (PadManager.Instance.anyPawnOnTheWay(pawn, step))
        {
            if (PadManager.Instance.isAttackableEnemyOnTheWay(pawn, step))
            {
                var currentPad = PadManager.Instance.getIndexOfPadByPawn(pawn);
                var targetPawn = PadManager.Instance.getPadAtIndex(currentPad,step).getPawnCaptured();
                if (!forPrediction)
                {
                    kickThePawn(targetPawn);
                }
            }
            else
            {
                if (!forPrediction)
                {
                    cannotMoveandPickAgain(pawn);
                    return;
                }
                else
                {
                    pawn.isCanNotMoveForPrediction = true;
                    return;
                }
            }
        }
        if (pawn.isFinish || pawn.isMoving && !pawn.isOut && !pawn.isReady && pawn.isMoved)
        {
            if (!forPrediction)
            {
                cannotMoveandPickAgain(pawn);
                return;
            }
            else
            {
                pawn.isCanNotMoveForPrediction = true;
                return;
            }
        };
        
        if (!forPrediction)
        {
            DiceManager.Instance.resetToCenter();
            var tasks = new Task[step];
            for (int i = 0; i < step; i++)
            {
                int currentPadIndex = PadManager.Instance.getIndexOfPadByPawn(pawn);
                Pad destinationPad = PadManager.Instance.getPadAtIndex(currentPadIndex, 1);
                //set new position
                pawn.setCurrentPad(destinationPad);

                //animation
                pawn.isMoving = true;

                tasks[i] = pawn.transform.DOJump(destinationPad.actualPos, 1, 1, movingTimeSpent).AsyncWaitForCompletion();

                await Task.WhenAll(tasks[i]);
                
                playPawningSound();
            

            
                rotateIfNeed(pawn);
                pawn.isMoving = false;
            }
            pawn.isMoved = true;
            endTurn(pawn.isMoved);
        }
        
    }

    private async Task checkGatePoint(Pawn pawn, bool forPrediction = false)
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

        if (pawn.isFinish)
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
                if (!forPrediction)
                {
                    cannotMoveandPickAgain(pawn);
                    return;
                }
                else
                {
                    pawn.isCanNotMoveForPrediction = true;
                    return;
                }
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

        if (!forPrediction)
        {
            if (!pawn.isMoving && pawn.isOut && pawn.isReady && firstEmptyInx > 0 && !pawn.isMoved)
            {
            
                DiceManager.Instance.resetToCenter();
                Pad destinationPad = GPads[firstEmptyInx];
                pawn.setCurrentPad(destinationPad);
                pawn.isMoving = true;
                await pawn.transform.DOJump(destinationPad.actualPos, 3, 1, 1.5f).SetEase(Ease.InSine).OnComplete(() =>
                {
                    Debug.Log("GATED!");
                    playPawningSound();
                    rotateIfNeed(pawn);
                    pawn.isMoving = false;
                    pawn.isMoved = true;
                    pawn.isFinish = true;
                    endTurn(pawn.isMoved);
                }).AsyncWaitForCompletion();
            
            }
        }
    }

    private async Task checkOut(Pawn pawn, bool forPrediction = false)
    {
        if (DiceManager.Instance.anyOneOrSix())
        {
            var startPad = PadManager.Instance.getSpawnPadByTeam(pawn.Team);
            if (startPad && !startPad.isFree)
            {
                if (startPad.getPawnCaptured().Team != pawn.Team)
                {
                    if (!forPrediction)
                    {
                        kickThePawn(startPad.getPawnCaptured());
                    }
                }
                else
                {
                    if (!forPrediction)
                    {
                        cannotMoveandPickAgain(pawn);
                        return;
                    }
                    else
                    {
                        pawn.isCanNotMoveForPrediction = true;
                        return;
                    }
                }
            }

            if (!forPrediction)
            {
                await moveToSpawn(pawn);
            
            
                rotateIfNeed(pawn);
                if (DiceManager.Instance.allOneOrSix())
                {
                    GameManager.Instance.updateGameState(GameManager.GameState.RollTheDice);
                }
                else
                {
                    endTurn(pawn.isMoved);
                }
            }
        }
        else
        {
            if (!forPrediction)
            {
                cannotMoveandPickAgain(pawn);
            }
            else
            {
                pawn.isCanNotMoveForPrediction = true;
            }
        }
    }

    public void rotateIfNeed(Pawn pawn)
    {
        if (pawn.isReady)
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
        if ((pawn.getCurrentPad().isGate || pawn.isFinish) && pawn.getCurrentPad().padTeam.Contains(pawn.Team.teamName))
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

    private async void kickThePawn(Pawn kickedPawn)
    {
        kickedPawn.setCurrentPad(null);
        await kickedPawn.sentToHome(1f);
    }

    private async Task moveToSpawn(Pawn pawn)
    {
        var startPad = PadManager.Instance.getSpawnPadByTeam(pawn.Team);
        if (startPad)
        {
            pawn.setCurrentPad(startPad);
            await pawn.recoveryToCurrentPad(.3f);
            playPawningSound();
            pawn.isMoved = true;
        }
        await Task.WhenAll();
    }

    private void playPawningSound()
    {
        int soundCount = SettingManager.Instance.audioCatalog.pawning.Count;

        if (soundCount < 1)
        {
            Debug.LogError("no pawning sound");
            return;
        }

        int random = Random.Range(0, soundCount);
        SettingManager.Instance.playEffect(SettingManager.Instance.audioCatalog.pawning[random]);
    }
}
