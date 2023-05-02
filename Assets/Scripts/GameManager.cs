using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] protected List<Team> listTeam;
    public GameState state;
    public Team currentTeam;

    public enum GameState
    {
        SelectColor,
        RollTheDice,
        PickAPawn,
        Pawning,
        SwitchTeam,
    }
    public static event Action<GameState> OnBeforeGameStateChanged;
    public static event Action<GameState> OnAfterGameStateChanged;

    private void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        updateListTeamUnFinish();
        
        updateGameState(GameState.SwitchTeam);
    }
    
    void Update()
    {
    }

    private async void updateListTeamUnFinish()
    {
        if (TeamManager.Instance)
        {
            listTeam = TeamManager.Instance.getUnFinishTeams();
        }
    }

    public async void updateGameState(GameState newState,Team switchedTeam = null)
    {
        state = newState;
        OnBeforeGameStateChanged?.Invoke(newState);
        switch (newState)
        {
            case GameState.SelectColor:
                break;
            case GameState.RollTheDice:
                DiceManager.Instance.isRolled = false;
                break;
            case GameState.PickAPawn:
                List<Pawn> testingPawns = currentTeam.pawns;
                int testDiceResult = DiceManager.Instance.totalResult;
                foreach (var pawn in testingPawns)
                {
                    PawnManager.Instance.pawning(pawn,testDiceResult,true);
                }
                if (testingPawns.All(pawn => pawn.isCanNotMoveForPrediction == true))
                {
                    Debug.Log("Auto Skip");
                    skipTurn();
                }
                resetPrediction(testingPawns);
                break;
            case GameState.Pawning:
                Pawn theChosenOne = SelectionManager.Instance.pawnSelected;
                int diceResult = DiceManager.Instance.totalResult;
                if (theChosenOne && theChosenOne.Team == currentTeam)
                {
                    PawnManager.Instance.pawning(theChosenOne,diceResult);
                }
                else
                {
                    SelectionManager.Instance.emptyPawnSelection();
                    updateGameState(GameState.PickAPawn);
                }
                break;
            case GameState.SwitchTeam:
                
                nextTeam();
                
                if (listTeam.Count < 1)
                {
                    Debug.Log("No team found!");
                    return;
                }
                else
                {
                    if (!currentTeam)
                    {
                        Debug.Log("unknown current Team \n Set to first team");
                        currentTeam = listTeam[0];
                    }
                }
                
                updateListTeamUnFinish();

                updateGameState(GameState.RollTheDice);
                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        OnAfterGameStateChanged?.Invoke(newState);
    }

    public void skipTurn()
    {
        updateGameState(GameState.SwitchTeam);
    }

    private void nextTeam()
    {
        if (!currentTeam)
        {
            Debug.Log("unknown current Team");
            return;
        }
        int currentTeamInx = listTeam.IndexOf(currentTeam);
        if (currentTeam == listTeam.Last())
        {
            currentTeam = listTeam[0];
        }
        else
        {
            currentTeam = listTeam[currentTeamInx + 1];
        }
    }

    private void resetPrediction(List<Pawn> pawns)
    {
        foreach (var pawn in pawns)
        {
            pawn.isCanNotMoveForPrediction = false;
        }
    }
}
