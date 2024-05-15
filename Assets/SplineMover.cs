using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMover : MonoBehaviour
{
    public Spline spline;

    public float speed = 3.0f;
    private int splinePasses;
    private bool complete;
    private bool start;

    PlayerLocomotion playerLocomotion;

    public GameObject mainCamera;
    public GameObject platformCam;

    void Start()
    {
       
        splinePasses = 0;
        complete = false;
        start = false;
        platformCam.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerLocomotion = other.GetComponent<PlayerLocomotion>();
            other.transform.parent = transform;
            start = true;
            other.GetComponent<Rigidbody>().isKinematic = false;
            
        }
        
       
    }


    void Update()
    {
        if (start)
        {
            if (complete == false)
            {
                mainCamera.SetActive(false);
                platformCam.SetActive(true);
                if (transform.position == spline.splinePoint[splinePasses])
                {
                    if (splinePasses < spline.splineCount -1 )
                    {
                        splinePasses++;
                    }
                    else
                    {
                        Debug.Log("HERE");
                        mainCamera.SetActive(true);
                        platformCam.SetActive(false);
                        complete = true;

                    }
                }
                playerLocomotion.isGrounded = true;
                transform.position = Vector3.MoveTowards(transform.position, spline.splinePoint[splinePasses], speed * Time.deltaTime);

            }
        }
    }
}
