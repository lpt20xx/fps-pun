using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Burst.CompilerServices;

public class SingleShotGun : Gun
{
    [SerializeField] private Camera cam;
    private PhotonView gunPhotonView;

    public TrailRenderer tracerEffect;

    private void Awake()
    {
        gunPhotonView = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Debug.Log("Using gun " + itemInfo.itemName);
        Shoot();
    }

    [SerializeField] private Transform raycastOrigin;

    private Ray ray2;

    public RaycastHit hit;

    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        

        

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("We hit " + hit.collider.gameObject.name);

            Debug.DrawLine(raycastOrigin.position, hit.point, Color.red, 1.0f);
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            gunPhotonView.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            

        }

    }

    [PunRPC]
    private void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        ray2.origin = raycastOrigin.position;

        var tracer = Instantiate(tracerEffect, ray2.origin, Quaternion.identity);
        tracer.AddPosition(ray2.origin);

        tracer.transform.position = hit.point;

        //Bullet Impacts
        Debug.Log("We hit position " + hitPosition);
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            bulletImpactObj.transform.SetParent(colliders[0].transform);

            //Destroy bulletImpactObj after 3 seconds
            Destroy(bulletImpactObj, 3f);
            
        }
        
    }
}
