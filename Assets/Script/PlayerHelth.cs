using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHelth : MonoBehaviour
{
     public float maxHealth = 100f;
    private float currentHealth;

    public RectTransform healthBarFill;
    private Animator anim;

    public GameObject player;


    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        anim = player.GetComponent<Animator>();
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

    anim.SetBool("diePlayer", false);

    gameObject.SetActive(true);
}


    IEnumerator DelayedDeath()
    {
        anim.SetBool("diePlayer", true);
        yield return new WaitForSeconds(2); 
        StartCoroutine(DieAndRespawn()); 
    }

}
