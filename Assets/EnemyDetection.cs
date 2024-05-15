using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{   
    InputManager inputManager;
    public GameObject enemyInRange;


    public Material Material1;
    public Material Material2;
    private void Start()
    {
        inputManager= GetComponentInParent<InputManager>();
    }
    
     void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetComponent<MeshRenderer>().material = Material1;
            
            inputManager.SetEnemyInRange(collision.gameObject);
            
        }

    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetComponent<MeshRenderer>().material = Material2;

            inputManager.SetEnemyInRange(null);

        }

    }
}
