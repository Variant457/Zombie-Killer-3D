using System.Diagnostics;
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

    // Profiling Variables
    [SerializeField] private ProfilerDataExporter profilerExporter;

    void Start()
    {
        {
            points = 0;
        }
    }

    // Changes the screen to the Fail Screen when the game ends
    public void GameOver()
    {
        var stopwatch = Stopwatch.StartNew();
        {
            healthScreen.enabled = false;
            scoreText.SetText("Your Score: " + points.ToString());
            failScreen.enabled = true;
            Time.timeScale = 0f;

            //Enables Cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GameManager.GameOver", stopwatch.Elapsed.TotalMilliseconds);
    }

    // Resets the game when the Play Again button has been pressed
    public void OnClick()
    {
        var stopwatch = Stopwatch.StartNew();
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
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GameManager.OnClick", stopwatch.Elapsed.TotalMilliseconds);
    }

    // Adds points to the global score
    public void AddPoints()
    {
        var stopwatch = Stopwatch.StartNew();
        {
            points += 100;
            pointText.SetText(points.ToString());
        }
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GameManager.AddPoints", stopwatch.Elapsed.TotalMilliseconds);
    }
}
