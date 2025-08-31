using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingAi : MonoBehaviour
{
    [SerializeField] private float StartingHealth;
    private float health;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
