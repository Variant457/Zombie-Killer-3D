using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static float maxHealth = 100;
    public int dmgAmt = 5;
    private float health = maxHealth;

    [SerializeField]
    private HealthBarUI healthBar;
    [SerializeField]
    private GameManager gameManager;

    void Start()
    {
        gameManager.failScreen.enabled = false;
        Debug.Log("Game Started");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected");
        if (collision.gameObject.CompareTag("Enemy") && health > 0)
        {
            health -= dmgAmt;
            healthBar.UpdateHealthBar(health, maxHealth);
            Debug.Log("Took a hit. Health - " + health);
            if (health <= 0)
            {
                gameManager.GameOver();
            }
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0 && health < maxHealth)
        {
            health += 0.001f;
            healthBar.UpdateHealthBar(health, maxHealth);
        }
    }
}
