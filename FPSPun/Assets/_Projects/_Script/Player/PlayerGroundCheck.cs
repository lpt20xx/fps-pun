using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerController playerContrroller;
    private void Awake()
    {
        playerContrroller = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerContrroller.gameObject)
        {
            return;
        }

        playerContrroller.SetGroundedState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerContrroller.gameObject)
        {
            return;
        }

        playerContrroller.SetGroundedState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerContrroller.gameObject)
        {
            return;
        }

        playerContrroller.SetGroundedState(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerContrroller.gameObject)
        {
            return;
        }

        playerContrroller.SetGroundedState(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerContrroller.gameObject)
        {
            return;
        }

        playerContrroller.SetGroundedState(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerContrroller.gameObject)
        {
            return;
        }

        playerContrroller.SetGroundedState(true);
    }
}
