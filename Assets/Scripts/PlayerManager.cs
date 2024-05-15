using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    AnimatorManager animatorManager;
    Animator animator;
    
    public CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;
    public Vector3 spawnPoint;
    public bool isInteracting;
    public int health;

    private void Awake()
    {
        spawnPoint = new Vector3(28.9899998f, 1.37f, -40.9000015f);
        health = 15;
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        //cameraManager = GetComponent<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        if (Input.GetKeyDown(KeyCode.P))
        {
            
            animator.Play("Dance");
            
        }

    }

    public void DoDamage()
    {
        Debug.Log(health);
        health = health - 1;
        animatorManager.PlayTargetAnimation("GetHit", false);
        health = health - 1;
        
        
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
        if (health < 0)
        {
            transform.position = spawnPoint;
            health = 15;
        }
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded",playerLocomotion.isGrounded);
    }
}
