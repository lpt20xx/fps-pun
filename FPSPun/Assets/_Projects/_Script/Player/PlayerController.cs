using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cameraHolder;

    [Header("-------Look-------")]
    [SerializeField] private float mouseSensitivity = 3f;

    [Header("-------Move-------")]
    [SerializeField] private float sprintSpeed      = 6f;
    [SerializeField] private float walkSpeed        = 3f;

    [Header("-------Jump-------")]
    [SerializeField] private float jumpForce        = 300f;
    [SerializeField] private float smoothTime       = 0.15f;

    private float verticalLookRotation;
    [SerializeField] private bool grounded;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;

    Rigidbody rb;

    public PhotonView playerPhotonView;

    [Header("-------Equip Item-------")]
    [SerializeField] private PlayerItems playerItems;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerPhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (playerPhotonView.IsMine)
        {
            playerItems.EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    private void Update()
    {
        if(!playerPhotonView.IsMine) 
            return;

        Look();
        Move();
        Jump();

        SwitchWeapon();
        UseWeapon();
    }

    private void UseWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerItems.items[playerItems.itemIndex].Use();
        }
    }

    private void SwitchWeapon()
    {
        //press number key to switch weapon
        for (int i = 0; i < playerItems.items.Length; i++)
        if(Input.GetKeyDown((i + 1).ToString()))
        {
            playerItems.EquipItem(i);
            break;
        }

        //Use mouse scroll wheel to switch weapon
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if(playerItems.itemIndex >= playerItems.items.Length - 1)
            {
                playerItems.EquipItem(0);
            }
            else
            {
                playerItems.EquipItem(playerItems.itemIndex + 1);
            }
            
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (playerItems.itemIndex <= 0)
            {
                playerItems.EquipItem(playerItems.items.Length - 1);
            }
            else
            {
                playerItems.EquipItem(playerItems.itemIndex - 1);
            }
        }
    }

    //Syncing Gun Switching
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (!playerPhotonView.IsMine && targetPlayer == playerPhotonView.Owner)
        {
            playerItems.EquipItem((int)changedProps["itemIndex"]);
        }
    }

    private void Look()
    {
        //rotate camera horizontal
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        //rotate camera vertical
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        //Sprint
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundedState(bool grounded)
    {
        this.grounded = grounded;
    }

    private void FixedUpdate()
    {
        if (!playerPhotonView.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
