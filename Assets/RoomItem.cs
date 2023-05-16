using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    private string roomName;
    private TextMeshProUGUI roomNameUI;

    private void Awake()
    {
        roomNameUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        roomName = roomNameUI.text;
    }

    private void Update()
    {
        
    }

    private void UpdateRoom()
    {
        roomNameUI.text = roomName;
    }

    private void SetRoomName(string name)
    {
        this.roomName = name;
    }

    public void SetRoomItemData(Room room)
    {
        this.SetRoomName(room.roomName);
        
        
        UpdateRoom();
    }
}
