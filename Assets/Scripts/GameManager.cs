using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas healthScreen;
    public Canvas failScreen;

    public void GameOver()
    {
        healthScreen.enabled = false;
        failScreen.enabled = true;
    }
}
