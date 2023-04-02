using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathData", menuName = "LPath/PathSO", order = 1)]
public class PathSO : ScriptableObject
{
    public string objectName = "New MyScriptableObject";
    public Pad[] Team1Pads = new Pad[14];
    
    public Pad[] Team2Pads = new Pad[14];

    public Pad[] Team3Pads = new Pad[14];

    public Pad[] Team4Pads = new Pad[14];

}
