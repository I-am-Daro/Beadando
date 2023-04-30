using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   [SerializeField] float hitPoints = 100f;

   //public Weapon weapon;
   EnemyAI enemyAI;
   public CollisionHandler collisionHandler;

    void Awake() 
   {
    collisionHandler = GetComponent<CollisionHandler>();
    //weapon = FindObjectOfType<Weapon>();
    enemyAI = FindObjectOfType<EnemyAI>();
   }

    public void TakeDamage(float damage)
    {
      BroadcastMessage("OnDamageTaken");
      hitPoints -= damage;
      if(hitPoints <= 0)
      {
      GetComponent<Animator>().SetBool("killed", true);
      
      enemyAI.enabled = false;
      }
    }  
}
