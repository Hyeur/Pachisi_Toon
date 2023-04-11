using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;

    [FormerlySerializedAs("_diceList")] [SerializeField] protected List<Dice> diceList;
    //private float _result = 0;
    
    void Start()
    {
        DiceManager.Instance = this;
        
        foreach (Transform dice in GetComponentsInChildren<Transform>())
        {
            if (dice.CompareTag("Dice"))
            {
                diceList.Add(dice.GetComponent<Dice>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getTotalResult()
    {
        if (!isAllDiceStable())
            return 0;
        float result = 0;
        foreach (Dice dice in diceList)
        {
            result += dice.getTossResult();
        }
        resetToCenter();
        return result;
    }

    public void showResult()
    {
        getTotalResult();
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

    private IEnumerator tossTheDiceByTeam(Transform team)
    {
        foreach (Dice dice in diceList) dice.setVisible(false);
        foreach (Dice dice in diceList)
        {
            dice.setVisible(true);
            tossDice(dice.gameObject,team.transform.position);
            yield return new WaitForSeconds(.5f);
        }
    }

    public void toss(Transform team)
    {
        StartCoroutine(tossTheDiceByTeam(team));
    }

    private void tossDice(GameObject diceObject,Vector3 direction)
    {
        Vector3 torque = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        diceObject.transform.position = direction;
        diceObject.transform.rotation = Quaternion.Euler(torque.x,torque.y,torque.z);
        diceObject.GetComponent<Dice>().getRb().velocity = ((new Vector3(0,direction.y + 12,0) - direction)) * 1f;
        diceObject.GetComponent<Dice>().getRb().AddTorque(torque * 30);
    }

    public void resetToCenter()
    {
        Vector3 reset = new Vector3(0, 1, -1);
        foreach (Dice dice in diceList)
        {
            dice.transform.position = reset;
            reset.z += 1;
        }
    }
}
