using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameBillboard : MonoBehaviour
{
    private Camera cam;

    // Update is called once per frame
    private void Update()
    {
        if(cam == null)
        {
            cam = FindObjectOfType<Camera>();
        }

        if(cam == null)
        {
            return;
        }

        transform.LookAt(cam.transform);
        //transform.Rotate(Vector3.up * 100);
    }
}
