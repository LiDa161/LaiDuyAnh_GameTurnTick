using UnityEngine;
using System.Collections.Generic;

public class CheckWinLevelUp : MonoBehaviour
{
    private bool gameEnded = false;

    public Canvas clearCanvas;

    public List<GameObject> bellOffList = new List<GameObject>();
    public List<GameObject> bellOnList = new List<GameObject>();

    public AudioClip clearSound;
    public AudioClip bellOnSound;
    public AudioClip bellOffSound;

    public float bellCooldown = 1f;

    private float lastGlobalBellInteractionTime = -10f;

    private GameModeManager gameModeManager;

    void Start()
    {
        for (int i = 0; i < bellOffList.Count; i++)
        {
            if (bellOffList[i] != null) bellOffList[i].SetActive(true);
            if (i < bellOnList.Count && bellOnList[i] != null) bellOnList[i].SetActive(false);
        }

        gameModeManager = FindObjectOfType<GameModeManager>();

        if (gameModeManager == null)
        {
            Debug.LogWarning("Không tìm thấy GameModeManager trong scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded) return;

        float currentTime = Time.time;

        if (currentTime - lastGlobalBellInteractionTime < bellCooldown)
        {
            Debug.Log("Cooldown toàn bộ chuông đang hoạt động");
            return;
        }

        for (int i = 0; i < bellOffList.Count; i++)
        {
            GameObject off = bellOffList[i];
            GameObject on = bellOnList[i];

            if (other.gameObject == off)
            {
                lastGlobalBellInteractionTime = currentTime;

                off.SetActive(false);
                if (on != null) on.SetActive(true);

                if (bellOnSound != null)
                    AudioSource.PlayClipAtPoint(bellOnSound, Camera.main.transform.position);

                Debug.Log($"Bell {i + 1} bật");
                return;
            }

            if (other.gameObject == on)
            {
                lastGlobalBellInteractionTime = currentTime;

                on.SetActive(false);
                if (off != null) off.SetActive(true);

                if (bellOffSound != null)
                    AudioSource.PlayClipAtPoint(bellOffSound, Camera.main.transform.position);

                Debug.Log($"Bell {i + 1} tắt");
                return;
            }
        }

        if (other.CompareTag("Goal"))
        {
            if (!AllBellsActivated())
            {
                Debug.Log("❌ Chưa bật hết chuông → chưa thể thắng");
                return;
            }

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
                    Debug.Log("YOU WIN!");
                }
                else
                {
                    Debug.LogWarning("Chưa gán clearCanvas!");
                }
            }
        }
    }

    bool AllBellsActivated()
    {
        foreach (GameObject bellOn in bellOnList)
        {
            if (bellOn == null || !bellOn.activeSelf)
                return false;
        }
        return true;
    }
}
