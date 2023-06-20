using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerName;

    Player playerInfo;

    public void SetUp(Player player)
    {
        playerInfo = player;
        playerName.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (playerInfo ==  otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
