using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float zombieSpeed = 5f;
    public Transform playerTransform;

    void Update()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * zombieSpeed * Time.deltaTime;
    }
}
