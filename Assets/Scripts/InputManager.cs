using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public GameObject enemyInRange;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprintInput;
    public bool jumpInput;
    public bool interactInput;
    

    private void Awake()
    {
        
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Interact.performed += i => interactInput = true;


        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    
    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleInteractInput();
        //ACTION
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (sprintInput && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jumpInput)
        {
            jumpInput= false;
            playerLocomotion.HandleJumping();
        }
    }

    public void HandleInteractInput()
    {
        if (interactInput) {
            
            int random = Random.Range(0, 4);
            if (random == 0)
            {
                animatorManager.PlayTargetAnimation("Attack", false);

            }
            else if (random == 1) 
            { 
                animatorManager.PlayTargetAnimation("Attack 2", false); 
            }
            else if (random == 2)
            {
                animatorManager.PlayTargetAnimation("Attack 3", false);
            }
            else if (random == 3)
            {
                animatorManager.PlayTargetAnimation("Attack 4", false);
            }
            if (enemyInRange != null)
            {
               
                enemyInRange.GetComponent<EnemyAI>().Hit();
            }
            
            interactInput = false;
        }
    }

    public void ResetAnim()
    {
        interactInput = false;
    }

   public void SetEnemyInRange(GameObject enemy)
    {
       
        enemyInRange = enemy;
    }
}
