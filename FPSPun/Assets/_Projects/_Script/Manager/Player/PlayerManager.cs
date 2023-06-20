using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        Debug.Log("Instantiated our player controller");
        //Instantiate our player controller
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}
