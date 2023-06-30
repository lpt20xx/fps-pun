using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerTakeDamage : MonoBehaviour, IDamageable
{
    private PlayerController playerController;

    private PlayerManager playerManager;

    [Header("-------Player Health-------")]
    private const float maxHealth = 100f;
    private float currentHealth = maxHealth;

    [SerializeField] private Image healthBarImage;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerManager = PhotonView.Find((int)playerController.playerPhotonView.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Deal Damage: " + damage);
        playerController.playerPhotonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage) 
    {
        if (!playerController.playerPhotonView.IsMine)
        {
            return;
        }

        Debug.Log("Took Damage: " + damage);

        currentHealth -= damage;

        healthBarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0) 
        {
            Die();
        }
    }

    private void Die()
    {
        playerManager.Die();
    }
}
