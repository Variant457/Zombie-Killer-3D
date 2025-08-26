using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public float width, height;
    private float maxHealth = 100f;
    private float health = 100f;

    [SerializeField]
    private RectTransform healthBar;
}
