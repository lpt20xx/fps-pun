using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerItems : MonoBehaviour
{
    public Item[] items;

    public int itemIndex;
    int previousItemIndex = -1;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void EquipItem(int index)
    {
        if(index ==  previousItemIndex)
        {
            return;
        }

        itemIndex = index;

        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1) 
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = index;

        //Syncing Gun Switching
        if (playerController.playerPhotonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
}
