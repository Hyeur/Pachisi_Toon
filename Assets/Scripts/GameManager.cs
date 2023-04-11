using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected Team[] ListTeam;
    void Start()
    {
        ListTeam = GameObject.FindObjectsOfType<Team>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
