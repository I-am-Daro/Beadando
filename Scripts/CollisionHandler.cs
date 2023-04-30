using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Weapon weapon;
    public GameObject hitParticle;
    public EnemyHealth enemyHealth;
    
    void Awake()
        {
            GameObject otherGameObject = GameObject.FindWithTag("Enemy");
            enemyHealth = otherGameObject.GetComponent<EnemyHealth>();
        }
    void OnTriggerEnter(Collider other) 
    {
        

        if(other.tag == "Enemy" && weapon.isAttacking)
        {            
            //other.GetComponentInParent<Animator>().SetTrigger("getstruck");
            enemyHealth.TakeDamage(weapon.damage);
            Instantiate(hitParticle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), other.transform.rotation);
        } 

       
    }




}
