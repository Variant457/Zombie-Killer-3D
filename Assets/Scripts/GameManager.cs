using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas healthScreen;
    [SerializeField]
    private PlayerManager player;

    public Canvas failScreen;
    public Transform playerTransform;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI scoreText;
    private int points;

    void Start()
    {
        points = 0;
    }

    public void GameOver()
    {
        healthScreen.enabled = false;
        scoreText.SetText("Your Score: " + points.ToString());
        failScreen.enabled = true;
        Time.timeScale = 0f;

        //Enables Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClick()
    {
        Time.timeScale = 1f;

        //Destroys all previous zombies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        playerTransform.position = new Vector3(0, 30, 0);

        //Relocks Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Resets Screens
        player.ResetHealth();
        points = 0;
        failScreen.enabled = false;
        pointText.SetText(points.ToString());
        healthScreen.enabled = true;
    }

    public void AddPoints()
    {
        points += 100;
        pointText.SetText(points.ToString());
    }
}
