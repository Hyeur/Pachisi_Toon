using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        updateListTeamUnFinish();
        currentTeam = listTeam[0];
    }
    
    void Update()
    {
        
    }

    public void updateListTeamUnFinish()
    {
        listTeam = TeamManager.Instance.getUnFinishTeams();
    }

    public void updateGameState(GameState newState)
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
                if (theChosenOne && theChosenOne._isOut && theChosenOne._isReady && theChosenOne.Team == currentTeam)
                {
                    PawnManager.Instance.movePawn(theChosenOne,diceResult);
                }
                else
                {
                    SelectionManager.Instance.emptyPawnSelection();
                    updateGameState(GameState.PickAPawn);
                }
                break;
            case GameState.SwitchTeam:
                int currentTeamInx = listTeam.IndexOf(currentTeam);
                if (currentTeamInx == listTeam.Count() - 1)
                {
                    currentTeam = listTeam[0];
                }
                else
                {
                    currentTeam = listTeam[currentTeamInx + 1];
                }
                updateListTeamUnFinish();
                updateGameState(GameState.RollTheDice);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        
    }
}
