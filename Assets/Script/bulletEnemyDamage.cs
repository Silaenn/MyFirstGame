using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletEnemyDamage : MonoBehaviour
{
    private float damage = 10f;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            PlayerHelth playerHelth = other.GetComponent<PlayerHelth>();
            if(playerHelth != null){
                playerHelth.TakeDamage(damage);
            }
        }
    }
  
}
