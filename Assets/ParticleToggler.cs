using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleToggler : MonoBehaviour
{
    public PlayerLocomotion playerLocomotion;
    private ParticleSystem smoker;
    
    void Start()
    {
        smoker= GetComponent<ParticleSystem>();
    }

   
    void Update()
    {
        if (playerLocomotion.speedBoost)
        {
            smoker.Play();
            
        }
        else { smoker.Pause(); }
        
    }
}
