using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float zombieSpeed = 5f;
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    void Update()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * zombieSpeed * Time.deltaTime;
    }
}
