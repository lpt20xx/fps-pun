using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private PhotonView playerPhotonView;
    [SerializeField] private TMP_Text txtPlayerName;

    private void Start()
    {
        if (playerPhotonView.IsMine)
        {
            //Disable player-self name
            gameObject.SetActive(false);
        }

        txtPlayerName.text = playerPhotonView.Owner.NickName;
    }
}
