using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;

    [SerializeField] protected List<Dice> diceList;
    //private float _result = 0;
    public bool isRolled;
    [SerializeField] private bool _active = false;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        _active = (state == GameManager.GameState.RollTheDice);
    }

    void Start()
    {
        DiceManager.Instance = this;
        scanAllDice();
    }
    
    void Update()
    {
        endTurnTracking();
    }

    private void scanAllDice()
    {
        foreach (Transform dice in GetComponentsInChildren<Transform>())
        {
            if (dice.CompareTag("Dice"))
            {
                diceList.Add(dice.GetComponent<Dice>());
            }
        }
    }

    public int getTotalResult(bool needResetDiceToEnter = false)
    {
        if (!isAllDiceStable())
            return 0;
        resetToCenter(needResetDiceToEnter);
        int result = 0;
        foreach (Dice dice in diceList)
        {
            result += dice.getTossResult();
        }
        return result;
    }

    public void showResult()
    {
        GameManager.Instance.updateGameState(GameManager.GameState.RollTheDice);
        getTotalResult();
    }
    private bool isAllDiceStable()
    {
        foreach (Dice dice in diceList)
        {
            if (!dice.isIdle())
            {
                //Debug.Log("All dice not stable");
                return false;
            }
        }
        return true;
    }

    private IEnumerator tossTheDiceByTeam(Team team)
    {
        foreach (Dice dice in diceList) dice.setVisible(false);
        foreach (Dice dice in diceList)
        {
            dice.setVisible(true);
            tossDice(dice.gameObject,team.transform.position);
            yield return new WaitForSeconds(.5f);
        }
    }

    public void toss(Team team)
    {
        if (_active)
        {
            StartCoroutine(tossTheDiceByTeam(team));
            isRolled = true;
        }
    }

    private void tossDice(GameObject diceObject,Vector3 direction)
    {
        Vector3 torque = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        diceObject.transform.position = direction;
        diceObject.transform.rotation = Quaternion.Euler(torque.x,torque.y,torque.z);
        diceObject.GetComponent<Dice>().getRb().velocity = ((new Vector3(0,direction.y + 12,0) - direction)) * 1f;
        diceObject.GetComponent<Dice>().getRb().AddTorque(torque * 30);
    }

    public void resetToCenter(bool needResetDiceToEnter)
    {
        if (!needResetDiceToEnter) return;
        Vector3 reset = new Vector3(0, 1, -1);
        foreach (Dice dice in diceList)
        {
            dice.transform.position = reset;
            reset.z += 1;
        }
    }

    private void endTurnTracking()
    {
        if (_active && isRolled)
        {
            if (isAllDiceStable())
            {
                PawnManager.Instance.resetAllPawn();
                GameManager.Instance.updateGameState(GameManager.GameState.PickAPawn);
            }
        }
    }
}
