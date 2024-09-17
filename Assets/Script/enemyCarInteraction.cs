using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCarInte : MonoBehaviour
{

    public GameObject enemyBiasa;
    public GameObject enemyGang;

    private EnemyHelth enemyHelth;
    private EnemyGangHelth enemyGangHelth;

       void OnTriggerEnter2D(Collider2D other) {
     if(other.gameObject.CompareTag("Enemy")){
        enemyHelth = other.GetComponent<EnemyHelth>();
        enemyGangHelth = other.GetComponent<EnemyGangHelth>();

         if (enemyHelth != null)
            {
                enemyHelth.TakeDamage(100);
            }
            else if (enemyGangHelth != null)
            {
                enemyGangHelth.TakeDamage(100);
            }
     }
    } 
}
