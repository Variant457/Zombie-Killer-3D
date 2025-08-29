using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBar;

    private float width;

    void Start()
    {
        width = healthBar.sizeDelta.x;
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        float healthPer = currentHP / maxHP;
        float newWidth = width * healthPer;

        healthBar.sizeDelta = new Vector2(newWidth, healthBar.sizeDelta.y);
    }
}
