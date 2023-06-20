using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;

    public RoomInfo roomInfo;
    public void SetUp(RoomInfo info)
    {
        roomInfo = info;
        roomName.text = info.Name;
    }

    public void OnClick()
    {
        RoomManager.Instance.JoinRoom(roomInfo);
    }
}
