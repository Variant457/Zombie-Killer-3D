using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public float health = 50f;
    public float zombieSpeed = 3f;
    private Transform playerTransform;
    private float maxHealth;

    void Start()
    {
        maxHealth = health;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    void Update()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * zombieSpeed * Time.deltaTime;

        if (Time.timeScale != 0) zombieSpeed += 0.0003f;

        if (health <= 0)
        {
            Destroy(gameObject);
            FindAnyObjectByType<GameManager>().AddPoints();
        }
    }

    public void TakeDmg(float amt)
    {
        health -= amt;
        GetComponent<ZombieHealthUI>().UpdateHealthUI(health, maxHealth);
    }
}
