using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class resetRotation : MonoBehaviour
{
    public PlayerLocomotion playerLocomotion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = transform;
            playerLocomotion.isGrounded = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            
            playerLocomotion.isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
            
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
