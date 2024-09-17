using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPlayerDamge : MonoBehaviour
{
    private float damage = 10f;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            EnemyHelth enemyHelth = other.GetComponent<EnemyHelth>();
            EnemyGangHelth enemyGangHelth = other.GetComponent<EnemyGangHelth>();
            if(enemyHelth != null){
                enemyHelth.TakeDamage(damage);
            }
            if(enemyGangHelth != null){
                enemyGangHelth.TakeDamage(damage);
            }
        }
    }
  
}
