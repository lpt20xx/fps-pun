using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [Header("-------Room-------")]
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomListItemPrefab;

    [Header("-------Join Room-------")]
    [SerializeField] private TMP_InputField roomNameInputFieldJoin;

    [Header("-------Player in Room-------")]
    [SerializeField] private Transform playerListContent;
    [SerializeField] private GameObject playerListItemPrefab;

    [Header("-------Start Game-------")]
    [SerializeField] private GameObject startGameButton;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateRoom()
    {
        Debug.Log("Create Room");

        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            CreateRoomWithRandomName();
        }
        else
        {
            PhotonNetwork.CreateRoom(roomNameInputField.text);
            MenuManager.Instance.OpenMenu("loading");
        }
    }


    //Create a random name for room
    private string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private string randomRoomName;
    private int characterLimit = 12;
    public void CreateRoomWithRandomName()
    {
        Debug.Log("Create Room With Random Name");

        for (int i = 0; i < characterLimit; i++)
        {
            randomRoomName += characters[Random.Range(0, characters.Length)];
        }

        PhotonNetwork.CreateRoom(randomRoomName);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name);

        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        //Instantiate player info when player join room
        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        

        //Set Start Game Button active if player is master client
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //Set Start Game Button active if master client is switched
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed");

        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed");

        errorText.text = "Join Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinButton()
    {
        string roomName = roomNameInputFieldJoin.text;

        if (string.IsNullOrEmpty(roomName))
        {
            return;
        }

        PhotonNetwork.JoinRoom(roomName);
        MenuManager.Instance.OpenMenu("loading");
    }

   

    public void CopyRoomName()
    {
        TextEditor roomNameCopied = new TextEditor();
        roomNameCopied.text = roomNameText.text;
        roomNameCopied.SelectAll();
        roomNameCopied.Copy();
    }

    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");

    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        MenuManager.Instance.OpenMenu("title");

        randomRoomName = "";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
