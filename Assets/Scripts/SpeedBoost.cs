using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;

    private void OnTriggerEnter(Collider other)
    {
        playerLocomotion = other.GetComponent<PlayerLocomotion>();
        playerLocomotion.speedBoost = true;
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Rotate( 60 * Time.deltaTime,0,0); 
    }

}
