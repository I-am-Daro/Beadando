using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandlerEnemy : MonoBehaviour
{

    public EnemyAI enemyAI;
    public GameObject hitParticle;
    public PlayerHealth playerHealth;

    void Awake()
    {
        GameObject otherGameObject = GameObject.FindWithTag("Player");
        playerHealth = otherGameObject.GetComponent<PlayerHealth>();
    }
    void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Enemy" && enemyAI.isAttacking)
        {
            //other.GetComponentInParent<Animator>().SetTrigger("getstruck");
            playerHealth.TakeDamage(enemyAI.damage);
            Instantiate(hitParticle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), other.transform.rotation);
        }

        else
        {
            return;
        }
    }

}
