using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxHealth;
    float currentHealth;
    public Slider slider;
    public Transform respawn;
    void Start()
    {
        currentHealth = maxHealth;
        slider.value = currentHealth / maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        slider.value = currentHealth / maxHealth;
        // Debug.Log("current health " + currentHealth);
        if(currentHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        if(respawn != null)
        {
            transform.position = respawn.position;
            currentHealth = maxHealth;
            slider.value = currentHealth / maxHealth;
            return;
        }
        Destroy(gameObject);
    }
}
