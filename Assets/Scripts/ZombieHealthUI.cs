using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthUI : MonoBehaviour
{
    private Transform cameraTransform;
    public Slider healthSlider;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
    }

    public void UpdateHealthUI(float update, float maxHealth) 
    {
        healthSlider.value = update / maxHealth;
    }
}
