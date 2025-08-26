using UnityEngine;

public class Player : MonoBehaviour
{
    public static float maxHealth = 100;
    public int dmgAmt = 5;         // Replace with Zombie Object
    private float health = maxHealth;

    [SerializeField]
    private HealthBarUI healthBar;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= dmgAmt;
            healthBar.width -= (100 - dmgAmt) * healthBar.width / 100;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= dmgAmt;
            healthBar.width -= (100 - dmgAmt) * healthBar.width / 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health < maxHealth) health += 0.01f;
    }
}
