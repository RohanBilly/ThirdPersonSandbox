using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;

    private void OnTriggerEnter(Collider other)
    {
        playerLocomotion = other.GetComponent<PlayerLocomotion>();
        playerLocomotion.doubleJump = true;
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Rotate(60 * Time.deltaTime, 20 * Time.deltaTime, 40 * Time.deltaTime);
    }

}
