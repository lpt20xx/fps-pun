using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    PhotonView photonView;

    GameObject controller;

    private void Awake()
    {
        Instance = this;
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
        
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();

        //Instantiate our player controller and respawn
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { photonView.ViewID });
    }

    public void Die()
    {
        Debug.Log("Die");
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
