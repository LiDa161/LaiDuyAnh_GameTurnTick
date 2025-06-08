using UnityEngine;

public class CheckWin : MonoBehaviour
{
    private bool gameEnded = false;

    public Canvas clearCanvas;
    public AudioClip clearSound;

    private GameModeManager gameModeManager;

    private void Start()
    {
        gameModeManager = FindObjectOfType<GameModeManager>();

        if (gameModeManager == null)
        {
            Debug.LogWarning("Không tìm thấy GameModeManager trong scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded) return;

        if (other.CompareTag("Goal"))
        {
            gameEnded = true;

            if (clearSound != null)
                AudioSource.PlayClipAtPoint(clearSound, Camera.main.transform.position);

            if (gameModeManager != null)
            {
                gameModeManager.TriggerWin();
            }
            else
            {
                if (clearCanvas != null)
                {
                    clearCanvas.gameObject.SetActive(true);
                    Debug.Log("You win!");
                }
            }
        }
    }
}
