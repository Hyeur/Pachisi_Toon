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
    public static event Action<GameState> OnGameStateChanged; 

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        updateGameState(GameState.RollTheDice);
    }
    
    void Update()
    {
        if (listTeam.Count < 1)
        {
            updateListTeamUnFinish();
        }
    }

    public async void updateListTeamUnFinish()
    {
        listTeam = TeamManager.Instance.getUnFinishTeams();
        if (!currentTeam && listTeam.Count > 0)
        {
            currentTeam = listTeam[0];
        }
    }

    public void updateGameState(GameState newState,Team switchedTeam = null)
    {
        state = newState;
        OnGameStateChanged?.Invoke(newState);
        switch (newState)
        {
            case GameState.SelectColor:
                break;
            case GameState.RollTheDice:
                DiceManager.Instance.isRolled = false;
                break;
            case GameState.PickAPawn:
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
                int currentTeamInx = listTeam.IndexOf(currentTeam);
                if (currentTeam == listTeam.Last())
                {
                    currentTeam = listTeam[0];
                }
                else
                {
                    currentTeam = listTeam[currentTeamInx + 1];
                }
                updateListTeamUnFinish();
                if (listTeam.Contains(switchedTeam) && switchedTeam)
                {
                    currentTeam = switchedTeam;
                }
                
                updateGameState(GameState.RollTheDice);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        
    }

    public void skipTurn()
    {
        updateGameState(GameState.SwitchTeam);
    }
}
