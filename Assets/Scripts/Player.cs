using UnityEngine;

public class Player : MonoBehaviour
{
    public static float maxHealth = 100;
    public int dmgAmt = 5;         // Replace with Zombie Object
    private float health = maxHealth;

    [SerializeField]
    private HealthBarUI healthBar;
    [SerializeField]
    private GameManager gameManager;

    void Start()
    {
        gameManager.failScreen.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
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
