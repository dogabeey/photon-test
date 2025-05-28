using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public GameObject lobbyObject;
    public GameObject roomObject;
    public TMP_InputField roomName; // Used for either joining or creating a room.
    public Button joinRoomButton;
    public Button createRoomButton;
    public Button disconnectButton; // Disconnects the user from the room.
    public TMP_Text errorText; // Text to display errors or messages to the user.
    // Start is called before the first frame update
    void Start()
    {
        // Disable UI until connected
        roomName.interactable = false;
        joinRoomButton.interactable = false;
        createRoomButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();

        joinRoomButton.onClick.AddListener(JoinRoom);
        createRoomButton.onClick.AddListener(CreateRoom);
        disconnectButton.onClick.AddListener(DisconnectFromRoom);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        // Enable UI after connection
        roomName.interactable = true;
        joinRoomButton.interactable = true;
        createRoomButton.interactable = true;
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        roomObject.SetActive(true);
        lobbyObject.SetActive(false);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        errorText.text = "Failed to join room: " + message;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        errorText.text = "Failed to create room: " + message;
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        roomObject.SetActive(false);
        lobbyObject.SetActive(true);
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            if(PhotonNetwork.JoinRoom(roomName.text))
            {
                Debug.Log("Joining room: " + roomName.text);
            }
            else
            {
                Debug.LogError("Failed to join room. Room name might be invalid or does not exist.");
            }
        }
        else
        {
            Debug.LogError("Not connected to Photon Network.");
        }
    }
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4; // Set the maximum number of players in the room
            if(PhotonNetwork.CreateRoom(roomName.text, roomOptions))
            {
                Debug.Log("Room created successfully.");
            }
            else
            {
                Debug.LogError("Failed to create room. Room name might be taken or invalid.");
            }
        }
        else
        {
            Debug.LogError("Not connected to Photon Network.");
        }
    }

    public void DisconnectFromRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Disconnected from room.");
        }
        else
        {
            Debug.LogError("Not connected to Photon Network.");
        }
    }
}
