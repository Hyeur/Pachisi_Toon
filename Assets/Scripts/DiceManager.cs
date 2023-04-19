using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;

    [SerializeField] protected List<Dice> diceList;
    public List<int> twoResult;
    public int totalResult = 0;
    public bool isRolled;
    [SerializeField] private bool _active = false;

    public TextMeshProUGUI r;
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
        Instance = this;
        scanAllDice();
    }
    
    void Update()
    {
        r.text = "Dice: " + totalResult;
        updateResult();
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

    private void updateResult()
    {
        if (isAllDiceStable() && totalResult == 0)
        {
            twoResult.Clear();
            foreach (Dice dice in diceList)
            {
                twoResult.Add(dice.getTossResult());
                totalResult += dice.getTossResult();
            }
        }
        
    }

    public void showResult()
    {
        GameManager.Instance.updateGameState(GameManager.GameState.RollTheDice);
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
        foreach (Dice dice in diceList)
        {
            dice.setVisible(false);
        }
        
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
            totalResult = 0;
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

    public async void resetToCenter()
    {
        float z = -1f;
        float offset = 0.58f;
        foreach (Dice dice in diceList)
        {
            dice.transform.position = Vector3.up * 15f;
            await dice.transform.DOMove(new Vector3(0,offset,z),1f).SetEase(Ease.OutExpo).AsyncWaitForCompletion();
            z += 2f;
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

    public List<int> getTwoResults()
    {
        return twoResult;
    }
}
