using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField playerNameInput;
    private void Start()
    {

        if (PlayerPrefs.HasKey("username"))
        {
            playerNameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            playerNameInput.text = "Player " + Random.Range(0, 1000).ToString("0000");
            OnPlayerNameInputValueChanged();
        }   
    }
    public void OnPlayerNameInputValueChanged()
    {
        PhotonNetwork.NickName = playerNameInput.text;
        PlayerPrefs.SetString("username", playerNameInput.text);
    }
}
