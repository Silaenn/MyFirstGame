using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHelth : MonoBehaviour
{
     public float maxHealth = 100f;
    private float currentHealth;

    public RectTransform healthBarFill;
    private Animator anim;

    public GameObject enemy;

    public Transform[] spawnPoints;

    public Vector2[] respawnLocations; // Array untuk lokasi respawn

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        anim = enemy.GetComponent<Animator>();
    }

  void UpdateHealthBar()
    {
        // Update ukuran health bar berdasarkan currentHealth
        if (healthBarFill != null)
        {
            float healthPercentage = currentHealth / maxHealth;
            healthBarFill.sizeDelta = new Vector2(maxHealth * healthPercentage, healthBarFill.sizeDelta.y);

            healthBarFill.anchoredPosition = new Vector2(maxHealth * (1 - healthPercentage), healthBarFill.anchoredPosition.y);
        }
        else
        {
            Debug.LogWarning("Health bar image reference is missing.");
        }
    }


    public void TakeDamage(float amount){
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if(currentHealth <= 0){
            StartCoroutine(DelayedDeath());
        }
    }

   IEnumerator DieAndRespawn()
{
    yield return new WaitForSeconds(0);

    gameObject.SetActive(false);


    currentHealth = maxHealth;
    UpdateHealthBar();

    RespawnAtRandomLocation();

    anim.SetBool("die", false);

    gameObject.SetActive(true);
}


    IEnumerator DelayedDeath()
    {
        anim.SetBool("die", true);
        yield return new WaitForSeconds(1); 
        StartCoroutine(DieAndRespawn()); 
    }

    void RespawnAtRandomLocation(){
        int randomIndex = Random.Range(0, respawnLocations.Length);
        transform.position = respawnLocations[randomIndex];
    }
}
