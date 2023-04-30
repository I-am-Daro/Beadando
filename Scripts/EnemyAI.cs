using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float stoppingDistance = 2f;
    [SerializeField] float turnSpeed = 3f;
    [SerializeField] float attackRange = 2.1f;
    [SerializeField] float attackSpeed = 3f;
    [SerializeField] public float damage = 40f;

    public PlayerKeymap playerKeymap;
    public Animator enemyAnim;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent navMeshAgent;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    public bool canAttack = true;
    public bool isAttacking = false;

    void OnDrawGizmosSelected() 
    {
       Gizmos.color = new Color(1, 0, 0, 1f);
       Gizmos.DrawWireSphere(transform.position, chaseRange); 
    }
    void Awake()
    {
        playerKeymap = new PlayerKeymap();
    }
    private void OnEnable()
    {
        playerKeymap.Enable();
    }

    private void OnDisable()
    {
        playerKeymap.Disable();
    }

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        playerHealth = FindObjectOfType<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
       
        if(isProvoked)
        {
            EngageTarget();
        }

        else if(distanceToTarget < chaseRange)
        {
            isProvoked = true;

            ChaseTarget();
            navMeshAgent.SetDestination(target.position);
        }
       
    }

    
    void EngageTarget()
    {
        if(distanceToTarget <= attackRange)
        {
            AttackTarget();
            

        }
        else
        {
            ChaseTarget();
        }

    }

     void ChaseTarget()
    {
        if (playerHealth.hitPoints > 0)
        {
            navMeshAgent.SetDestination(target.position);
            enemyAnim.SetBool("attack", false);
            enemyAnim.SetTrigger("move");
        }
    }

    void AttackTarget()
    {
        

        if (playerHealth.hitPoints > 0 && canAttack)
        {
            isAttacking = true;
            enemyAnim.SetBool("attack", true);
            if (playerKeymap.PlayerMovement.Deffense.ReadValue<float>() < 0.1f)
            {
                playerHealth.TakeDamage(damage);
            }
            canAttack = false;
            ResetAttack();
        }
        
    }

    void AttackReady()
    {
        canAttack = true;
    }

    void NowAttacking()
    {
        isAttacking = false;
    }

    void ResetAttack()
    {
        Invoke("AttackReady", attackSpeed);
        Invoke("NowAttacking", attackSpeed);
    }

        public void OnDamageTaken()
    {
        isProvoked = true;
    }

    public void PlayerDeath()
    {
         enemyAnim.SetBool("isdead", true);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

}
