using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    public PlayerKeymap playerKeymap;
    [SerializeField] float range = 10f;
    [SerializeField] public float damage = 30f;
    public GameObject BaseballBat;
    AudioSource audioSource;
    Animator anim;
    Animator batAnim;
    PlayerMovement playerM;
    public bool isMelee = true;
    public bool canAttack = true;
    public bool isAttacking = false;
    public float attackCooldown = 2f;
    public float timeToMoove = 1.3f;
    public EnemyHealth enemyHealth;
    Vector3 moveDirection = Vector3.zero;


    void Awake()
    {
        playerM = FindObjectOfType<PlayerMovement>();
        enemyHealth = FindObjectOfType<EnemyHealth>();
        anim = GetComponentInParent<Animator>();
        batAnim = GetComponentInChildren<Animator>();
        playerKeymap = new PlayerKeymap();
        //audioSource = GetComponent<AudioSource>();
        //audioSource.Stop();
    }
    private void OnEnable()
    {
        playerKeymap.Enable();
    }

    private void OnDisable()
    {
        playerKeymap.Disable();
    }

    void Update()
    {
        moveDirection = playerKeymap.PlayerMovement.WASD.ReadValue<Vector3>();

    }

    void FixedUpdate()
    {
        if (moveDirection == Vector3.zero)
        {
            OnAttackButtonPressed();
            OnDeffenseButtonPressed();
        }
    }

    void OnAttackButtonPressed()
    {
        bool isAttackPressed = playerKeymap.PlayerMovement.Attack.ReadValue<float>() > 0.1f;

        if (isAttackPressed)
        {
            playerM.enabled = false;
            SwingMeleeWeapon();
        }
        else
        {           
            anim.SetBool("attack", false);
            batAnim.SetBool("attack", false);
        }
    }

    void OnDeffenseButtonPressed()
    {
        bool isDeffensePressed = playerKeymap.PlayerMovement.Deffense.ReadValue<float>() > 0.1f;

        if (isDeffensePressed)
        {
            batAnim.SetBool("attack", false);
            playerM.enabled = false;
            DeffensiveStance();
        }
        else
        {           
            anim.SetBool("blocking", false);            
        }
    }

    public void SwingMeleeWeapon()
    {
        isAttacking = true;
        canAttack = false;
        
        if (!anim.GetBool("attack") && (!batAnim.GetBool("attack")) && (moveDirection == Vector3.zero))
        {
                anim.SetBool("attack", true);
                batAnim.SetBool("attack", true);
        }
        else
        {
            anim.SetBool("attack", false);
            batAnim.SetBool("attack", false);
        }
        StartCoroutine(WaitToMoove());
        ResetAttack();
    }

    public void DeffensiveStance()
    {        
        canAttack = false;
        if(!anim.GetBool("blocking"))
        {            
            anim.SetBool("blocking", true);
        }
        StartCoroutine(WaitToMoove());
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
        Invoke("AttackReady", attackCooldown);
        Invoke("NowAttacking", attackCooldown);
    }

    IEnumerator WaitToMoove()
    {
        yield return new WaitForSeconds(timeToMoove);
        if (playerKeymap.PlayerMovement.Deffense.ReadValue<float>() < 0.1f)
        {
            playerM.enabled = true;
        }
    }
}
