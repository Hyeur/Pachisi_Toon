using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Team : MonoBehaviour
{
    public static Team Instance;
    
    [SerializeField] private string _name = "TeamName";
    [SerializeField] private Pawn[] _pawns;
    [SerializeField] private bool isFinish = false;
    void Start()
    {
        Instance = this;

        _name = transform.name;
        _pawns = transform.GetComponentsInChildren<Pawn>();
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public bool Isfinish
    {
        get => isFinish;
        set => isFinish = value;
    }

    public int getNumsFinishPawn()
    {
        return _pawns.Count(x => x._isFinish);
    }
    public int getNumsOutPawn()
    {
        return _pawns.Count(x => x._isOut);
    }
}
