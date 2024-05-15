using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreassurePlate : MonoBehaviour
{
    public Door door;
    public Door door2;
    public string targetTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== targetTag) {
            door.Toggle();
            door2.Toggle();
        }
        
    }
}
