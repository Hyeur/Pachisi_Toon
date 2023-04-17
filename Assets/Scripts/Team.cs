using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Team : MonoBehaviour
{
    public string teamName = "TeamName";
    public List<Pawn> pawns;
    [SerializeField] private bool isFinish = false;
    void Start()
    {
        teamName = transform.name;

        pawns = GetComponentsInChildren<Pawn>().ToList();
    }

    public string Name
    {
        get => teamName;
        set => teamName = value;
    }

    public bool Isfinish
    {
        get => isFinish;
        set => isFinish = value;
    }

    public int getNumsFinishPawn()
    {
        return pawns.Count(x => x._isFinish);
    }
    public int getNumsOutPawn()
    {
        return pawns.Count(x => x._isOut);
    }

    public int getNumsPawn()
    {
        return pawns.Count();
    }
}
