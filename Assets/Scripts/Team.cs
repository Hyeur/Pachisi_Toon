using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Team : MonoBehaviour
{
    public string teamName = "TeamName";
    public List<Pawn> pawns;
    [SerializeField] private bool isWin = false;
    void Awake()
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
        get => isWin;
        set => isWin = value;
    }

    public int getNumsFinishPawn()
    {
        return pawns.Count(x => x.isFinish);
    }
    public int getNumsOutPawn()
    {
        return pawns.Count(x => x.isOut);
    }

    public int getNumsPawn()
    {
        return pawns.Count();
    }
}
