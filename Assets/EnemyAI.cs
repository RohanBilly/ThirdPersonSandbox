using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    private GameObject cubeMan;
    private float hitTimer;
    private bool attack;
    private float health;
    public bool dead;
    public float maxHealth;
    public bool split;
    public int attackRange;
    private UnityEngine.Vector3 attackPosition;
    private UnityEngine.Vector3 attackStartPoint;
    float pauseTimer;
    private float attackPause;
    private bool attackComplete;
    private bool wandering;
    private bool strafing;
    public bool doDamage;
    private float wanderSpeed;
    private float minWanderSpeed;
    private float maxWanderSpeed;
    private float minimumPonderLength;
    private float maximumPonderLength;
    private int maxWanderDistance;
    private Ray collisionRay;
    private UnityEngine.Vector3 wanderTarget;
    private UnityEngine.Vector3 wanderDirection;
    private float ponderTime;
    private float ponderLength;
    private float distanceFromPlayer;
    private GameObject player;
    
    private Transform playerPosition;
    private UnityEngine.Vector3 adjustedPlayerPosition;
    private Transform startPosition;
    
    private GameObject healthBarUI;
    private Slider slider;
    void Start()
    {
        if (attackRange == 0) { attackRange = 4; }
        dead = false;
        cubeMan = Resources.Load("CubeMan") as GameObject;
        healthBarUI = transform.GetComponentInChildren<Canvas>().gameObject;
        slider = healthBarUI.GetComponentInChildren(typeof(Slider)) as Slider;
        
        player = GameObject.FindGameObjectWithTag("Player");
        pauseTimer = 0.0f;
        health = maxHealth;
        slider.value = health / maxHealth;
        maxWanderDistance = 21;
        doDamage = false;
        attackComplete = false;

        attackPause = 1.0f;
        minWanderSpeed = 1.42f;
        maxWanderSpeed = 2.53f;
        minimumPonderLength = 3.62f;
        maximumPonderLength = 6.51f;


        attack = false;
        wandering = false;
        startPosition = transform;
        ponderLength = 0;
        hitTimer = 0;
    }

    public void Hit()
    {
        hitTimer += Time.deltaTime;

        health = health - 1;

        slider.value = health / maxHealth;
        if (health < 0)
        {
            dead = true;
            if (maxHealth == 20)
            {
                Debug.Log("HERE");
                player.GetComponent<PlayerManager>().spawnPoint = new UnityEngine.Vector3(-173.990005f, 86.8099976f, -242.669998f);
            }

                Destroy(gameObject);
                if (split)
                {
                    GameObject NewCubeA = Instantiate(cubeMan, new UnityEngine.Vector3(transform.position.x + 5, transform.position.y, transform.position.z), UnityEngine.Quaternion.identity);
                    NewCubeA.transform.localScale = new UnityEngine.Vector3(1.2f, 1.2f, 1.2f);
                    NewCubeA.GetComponent<EnemyAI>().maxHealth = 3;
                    NewCubeA.GetComponent<EnemyAI>().split = false;
                    GameObject NewCubeB = Instantiate(cubeMan, new UnityEngine.Vector3(transform.position.x - 5, transform.position.y, transform.position.z), UnityEngine.Quaternion.identity);
                    NewCubeB.transform.localScale = new UnityEngine.Vector3(1.2f, 1.2f, 1.2f);
                    NewCubeB.GetComponent<EnemyAI>().maxHealth = 3;
                    NewCubeB.GetComponent<EnemyAI>().split = false;
                }


            }
            if (health < maxHealth)
            {
                healthBarUI.SetActive(true);
            }
        }
    

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (doDamage)
            {
                player.GetComponent<PlayerManager>().DoDamage();
                doDamage = false;

            }
        }
        
         
    }

    private void Wander()
    {
        if (!wandering)
        {
            if (ponderTime < ponderLength)
            {
                ponderTime = ponderTime + Time.deltaTime;
            }
            else
            {
                wanderSpeed = Random.Range(minWanderSpeed, maxWanderSpeed);

                while (!wandering) 
                {
                    int wanderRangeX = Random.Range(-maxWanderDistance, maxWanderDistance);
                    int wanderRangeZ = Random.Range(-maxWanderDistance, maxWanderDistance);
                    float wanderDistance = Mathf.Sqrt((Mathf.Pow(wanderRangeX, 2)) + (Mathf.Pow(wanderRangeZ, 2))) + 4.5f;
                    
                    wanderTarget = new UnityEngine.Vector3(transform.position.x + wanderRangeX, transform.position.y, transform.position.z + wanderRangeZ);
                    wanderDirection = (transform.position - wanderTarget).normalized;
                    collisionRay = new Ray(transform.position, wanderDirection * -1);
                    
                    if (!Physics.Raycast(collisionRay, out RaycastHit hit, wanderDistance))
                    {
                        wandering = true;
                    }
                }
            }
        }
        else
        {
            
            transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, UnityEngine.Quaternion.LookRotation(wanderDirection), Time.deltaTime * 20f);
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, wanderTarget, wanderSpeed * Time.deltaTime);
            

            if (transform.position == wanderTarget) 
            {
                wandering = false;
                ponderLength = Random.Range(minimumPonderLength, maximumPonderLength);
                ponderTime = 0;
            }
        }
    }

    void Strafe()
    {
        while (!strafing)
        {
            int wanderRangeX = Random.Range(-5, 5);
            int wanderRangeZ = Random.Range(-5, 5);
            float wanderDistance = Mathf.Sqrt((Mathf.Pow(wanderRangeX, 2)) + (Mathf.Pow(wanderRangeZ, 2))) + 4.5f;

            wanderTarget = new UnityEngine.Vector3(transform.position.x + wanderRangeX, transform.position.y, transform.position.z + wanderRangeZ);
            
            collisionRay = new Ray(transform.position, wanderDirection * -1);

            if (!Physics.Raycast(collisionRay, out RaycastHit hit, wanderDistance))
            {
                strafing = true;
            }
        }
        if (strafing)
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            wanderDirection = (transform.position - playerPosition.position).normalized;
            transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, UnityEngine.Quaternion.LookRotation(wanderDirection), Time.deltaTime * 20f);
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, wanderTarget, wanderSpeed * Time.deltaTime);


            if (transform.position == wanderTarget)
            {
                strafing = false;
            }
        }
        
    }

    void Attack()
    {

        if (attackPause - 0.1f > pauseTimer)
        {

            pauseTimer += Time.deltaTime;
        }
        else if (attackPause> pauseTimer)
        {
            doDamage = true;
            pauseTimer += Time.deltaTime;
        }
        else if (!attackComplete)
        {

            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, attackPosition, 20 * Time.deltaTime);
            if (transform.position == attackPosition) { attackComplete = true; }
        }
        else if (attackComplete)
        {
            doDamage = false;
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, attackStartPoint, 15 * Time.deltaTime);
            if (transform.position == attackStartPoint)
            {
                attack = false;
                attackComplete = false;
                pauseTimer = 0.0f;
            }
        }
    }

    private void Update()
    {
        
        
        if (attack) Attack();

        else
        {
            distanceFromPlayer = UnityEngine.Vector3.Distance(player.transform.position, transform.position);
            playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            adjustedPlayerPosition = new UnityEngine.Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z);

            if (distanceFromPlayer < 20 && distanceFromPlayer > attackRange)
            {
                transform.position = UnityEngine.Vector3.MoveTowards(startPosition.position, adjustedPlayerPosition, 5 * Time.deltaTime);

                wanderDirection = (transform.position - adjustedPlayerPosition).normalized;
                transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, UnityEngine.Quaternion.LookRotation(wanderDirection), Time.deltaTime * 20f);


            }
            else if (distanceFromPlayer < attackRange)
            {
                attackPosition = adjustedPlayerPosition;
                attackStartPoint = transform.position;
                attack = true;
            }
            else
            {
                Wander();
            }
        }
    }
}
