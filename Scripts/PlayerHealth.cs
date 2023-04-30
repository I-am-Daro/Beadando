using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 200f;
    [SerializeField] TextMeshProUGUI healthAmounth;
    [SerializeField] Canvas hitIndicator;
    public float hitIndicatorLightupTime = 1f;
    public GameObject Player;


    DeathHandler deathHandler;
    EnemyAI enemyAI;
    PlayerMovement pM;
    
    void Awake()
    {
        deathHandler = FindObjectOfType<DeathHandler>();
        enemyAI = FindObjectOfType<EnemyAI>();
        pM = gameObject.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        DisplayHealthAmount();
    }

    void DisplayHealthAmount()
    {
        healthAmounth.text = hitPoints.ToString();
    }

    void DamageIndicatorOff()
    {
        hitIndicator.enabled = false;
    }
    void DamageIndicator()
    {
        hitIndicator.enabled = true;
        Invoke("DamageIndicatorOff", hitIndicatorLightupTime);
    }
    public void TakeDamage(float damage)
    {        
            DamageIndicator();
            hitPoints -= damage;

            if (hitPoints <= 0)
            {
                DamageIndicatorOff();
                deathHandler.HandleDeath();
                GetComponent<Animator>().SetBool("mooveforward", false);
                GetComponent<Animator>().SetBool("mooveback", false);
                GetComponent<Animator>().SetBool("mooveleft", false);
                GetComponent<Animator>().SetBool("mooveright", false);
                GetComponent<Animator>().SetBool("dead", true);
                enemyAI.SendMessage("PlayerDeath");
                Player.GetComponentInChildren<Rigidbody>().useGravity = true;
                Player.GetComponentInChildren<Animator>().enabled = false;
                Player.GetComponentInParent<PlayerMovement>().enabled = false;
                Player.GetComponentInChildren<Weapon>().enabled = false;
                enemyAI.enabled = false;
                pM.enabled = false;
            }        
    }
   

}
