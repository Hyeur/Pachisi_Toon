using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputPlayerName;
    public TMP_InputField inputRoomName;
    public List<RoomInfo> updatedRooms;
    public List<Room> clientRooms;

    public Transform roomContent;
    public RoomItem roomItemPrefabs;
    private void Start()
    {
        this.inputPlayerName.text = "Player1";
        this.inputRoomName.text = "Room1";
    }

    public void Login()
    {
        string playerName = this.inputPlayerName.text;
        Debug.Log(playerName + " logging");

        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Logout()
    {
        PhotonNetwork.Disconnect();
    }

    public void CreatedRoom()
    {
        string roomName = this.inputRoomName.text;
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom()
    {
        string roomName = this.inputRoomName.text;
        PhotonNetwork.JoinRoom(roomName);
    }

    private void AddRoom(RoomInfo roomInfo)
    {
        Room room = new Room(roomInfo.Name);
        
        this.clientRooms.Add(room);
    }

    private void RemoveRoom(RoomInfo roomInfo)
    {
        Room removingRoom = this.clientRooms.FirstOrDefault(r => r.roomName == roomInfo.Name);
        if (removingRoom == null) return;
        this.clientRooms.Remove(removingRoom);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        this.updatedRooms = roomList;

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) this.RemoveRoom(roomInfo);
            else this.AddRoom(roomInfo);
        }

        this.UpdateListRoomUI();
    }

    private void UpdateListRoomUI()
    {
        foreach (Transform child in this.roomContent)
        {
            Destroy(child);
        }

        foreach (Room room in clientRooms)
        {
            RoomItem newRoomItem = Instantiate(this.roomItemPrefabs, this.roomContent, true);
            newRoomItem.SetRoomItemData(room);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom " + PhotonNetwork.CurrentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom " + PhotonNetwork.CurrentRoom);
    }
    
    public override void OnLeftRoom()
    {
        Debug.Log( PhotonNetwork.NickName + " OnLeftRoom");
    }

    public override void OnJoinedLobby()
    {
        //Once player in lobby, they can fetch the PhotonListRoom update
        Debug.Log("OnJoinedLobby " + PhotonNetwork.CurrentLobby);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }
}
