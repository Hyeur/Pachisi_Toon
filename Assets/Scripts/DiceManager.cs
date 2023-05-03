using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
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
    
    [SerializeField] [Range(0,20)] private int tiltAngle = 0;
    [SerializeField] [Range(1, 3)] private float tossStrenght = 1;

    private void Awake()
    {
        GameManager.OnBeforeGameStateChanged += beforeGameManagerOnBeforeGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnBeforeGameStateChanged -= beforeGameManagerOnBeforeGameStateChanged;
    }

    private void beforeGameManagerOnBeforeGameStateChanged(GameManager.GameState state)
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
        updateResult();
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
        if (_active && isAllDiceStable() && isRolled)
        {
            twoResult.Clear();
            foreach (Dice dice in diceList)
            {
                twoResult.Add(dice.getTossResult());
                totalResult += dice.getTossResult();
            }
            endTurnTracking();
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
                Debug.Log("All dice not stable");
                return false;
            }
        }
        return true;
    }

    private async Task tossTheDiceByTeam(Team team)
    {
        foreach (Dice dice in diceList)
        {
            dice.setVisible(false);
        }
        
        foreach (Dice dice in diceList)
        {
            dice.setVisible(true);
            tossDice(dice.gameObject,team.transform.position);
            await Task.Delay(400);
        }
        
    }

    public async void toss(Team team)
    {
        if (_active)
        {
            await tossTheDiceByTeam(team);
            isRolled = true;
        }
    }

    private void tossDice(GameObject diceObject,Vector3 direction)
    {
        Vector3 torque = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        diceObject.transform.position = direction;
        diceObject.transform.rotation = Quaternion.Euler(torque.x, torque.y, torque.z);
        diceObject.GetComponent<Dice>().getRb().velocity =
            ((new Vector3(0, direction.y + tiltAngle, 0) - direction)) * tossStrenght;
        diceObject.GetComponent<Dice>().getRb().AddTorque(torque * 30);
    }

    public async void resetToCenter()
    {
        float z = -1f;
        float offset = 0.58f;
        Vector3 rootScale = diceList[0].transform.transform.localScale;
        foreach (Dice dice in diceList)
        {
            dice.getRb().useGravity = false;
            await dice.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InOutExpo).AsyncWaitForCompletion();
            await dice.transform.DOMove(new Vector3(0,offset,z),.1f).SetEase(Ease.OutExpo).AsyncWaitForCompletion();
            dice.getRb().useGravity = true;
            await dice.transform.DOScale(rootScale, .2f).SetEase(Ease.InOutExpo).AsyncWaitForCompletion();
            z += 2f;
        }
    }

    private void endTurnTracking()
    {
        if (_active)
        {
            Debug.Log("PickAPawn");
            PawnManager.Instance.resetAllPawn();
            GameManager.Instance.updateGameState(GameManager.GameState.PickAPawn);
        }
    }

    public List<int> getTwoResults()
    {
        return twoResult;
    }

    public bool anyOneOrSix()
    {
        if (twoResult.Any(result => result.Equals(1) || result.Equals(6))) return true;
        return false;
    }
    public bool allOneOrSix()
    {
        if (twoResult.All(result => result.Equals(1) || result.Equals(6))) return true;
        return false;
    }
}
