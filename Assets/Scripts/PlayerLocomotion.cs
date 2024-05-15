using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingSpeed;
    public float rayCastHeightOffset;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool speedBoost = false;
    public bool doubleJump = false;


    [Header("Movement Speeds")]
    public float walkingSpeed = 2.5f;
    public float runningSpeed = 7;
    public float sprintingSpeed = 12;
    public float rotationSpeed = 15;
    private float speedBoostTimer;

    [Header("Jump Speeds")]
    public float gravityIntensity;
    public float jumpHeight;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
}

    public void HandleAllMovement()
    {
      
        HandleFallingandLanding();

        if(playerManager.isInteracting)
        {
            return;
        }

        if (speedBoost) {
            walkingSpeed = 5;
            runningSpeed = 14;
            sprintingSpeed = 22;

            speedBoostTimer = speedBoostTimer + Time.deltaTime;
            if (speedBoostTimer > 7)
            {
                speedBoost = false;
                speedBoostTimer = 0;
            }
        }
        else {
            walkingSpeed = 2.5f;
            runningSpeed = 7;
            sprintingSpeed = 12;
        }

        HandleMovement();
        HandleRotation();
    }



    private void HandleMovement()
    {
        if (isJumping)
        {
            return;
        }

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }

        
        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
        if (isJumping)
        {
            playerRigidbody.velocity = playerRigidbody.velocity + movementVelocity;
            return;
        }
        else { playerRigidbody.velocity = movementVelocity; }
    }

    private void HandleRotation()
    {
        //if (isJumping)
        //{
        //    return;
        //}

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingandLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            //playerRigidbody.AddForce(transform.forward * leapingVelocity * 2);
            playerRigidbody.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
        }
        if(Physics.SphereCast(rayCastOrigin, 1.2f, -Vector3.up,out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                Vector3 movementVelocity = moveDirection;
                playerRigidbody.velocity = movementVelocity;
                animatorManager.PlayTargetAnimation("Land", true);
            }
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        //if (isGrounded && !isJumping)
        //{
        //    if(playerManager.isInteracting ||)
        //}
        
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }else if (doubleJump == true)
        {
            doubleJump = false;
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Flip", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight * 2.5f);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }
    }

    public void Interact()
    {

    }

}
