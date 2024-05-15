using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    PlayerManager playerManager;
    public GameObject enemyOne;
    EnemyAI enemyOneBrain;
    public GameObject enemyTwo;
    EnemyAI enemyTwoBrain;
    public bool open;
    private bool moving;
    private bool enemyOneDead;
    private bool enemyTwoDead;
    private int steps;
    public bool enemyDoor;

    
    private void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        enemyOneBrain = enemyOne.GetComponent<EnemyAI>();
        enemyTwoBrain = enemyTwo.GetComponent<EnemyAI>();
        enemyOneDead = false;
        enemyTwoDead = false;
        moving = false;
        steps = 0;
        
    }

    
    private void FixedUpdate()
    {
     if (enemyDoor)
        {
            if (enemyOneBrain.dead)
            {
                enemyOneDead = true;

            }
            if (enemyTwoBrain.dead)
            {
                enemyTwoDead = true;
            }
            if(enemyOneDead && enemyTwoDead)
            {
                Toggle();
                playerManager.spawnPoint = new Vector3(10.71f, 5.5f, -300.408997f);
                enemyDoor = false;
            }
        }   
        if(open == true && moving == true)
        {
            if (steps == 89)
            {
                moving = false;
                steps = 0;
            }
            transform.Rotate(0, 1, 0);
            steps = steps + 1;
        }
        if (open == false && moving == true)
        {
            if (steps == 89)
            {
                moving = false;
                steps = 0;
                
            }
            transform.Rotate(0, -1, 0);
            steps = steps + 1;
            
        }
    }

    public void Toggle()
    {
        Debug.Log(open);
        if (open == false && moving == false)
        {
            moving = true;
            open = true;
        }else if (open == true && moving == false)
        {
            moving = true;
            open = false;
        }

    }
    
}
