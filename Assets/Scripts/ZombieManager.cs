using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public float health = 50f;
    public float zombieSpeed = 5f;
    private Transform playerTransform;
    private GameManager gameManager;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    void Update()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * zombieSpeed * Time.deltaTime;

        if (Time.timeScale != 0) zombieSpeed += 0.0004f;

        if (health <= 0)
        {
            Destroy(gameObject);
            gameManager.AddPoints();
        }
    }
}
