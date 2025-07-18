﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameModeManager : MonoBehaviour
{
    [Header("Trang trí cho high score")]
    public GameObject highScoreDecoration;

    [Header("UI Canvas")]
    public GameObject canvasClear;
    public GameObject canvasFail;
    public GameObject canvasTime;

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;
    public float timeLimit = 60f;
    public float extraTimeAmount = 20f;

    [Header("Win Score UI")]
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    private float currentTime;
    private bool isTimerActive = false;
    private bool hasEnded = false;

    private int currentScore = 0;

    private string currentSceneName;
    private string highScoreKey;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        highScoreKey = "HighScore_" + currentSceneName;

        int mode = PlayerPrefs.GetInt("GameMode", 1);
        if (mode == 1)
        {
            currentTime = timeLimit;
            isTimerActive = true;
            canvasTime.SetActive(true);
            if (highScoreDecoration != null) highScoreDecoration.SetActive(true);
        }
        else
        {
            canvasTime.SetActive(false);
            if (highScoreDecoration != null) highScoreDecoration.SetActive(false);
        }

        canvasFail.SetActive(false);
        canvasClear.SetActive(false);
    }

    void Update()
    {
        if (!isTimerActive || hasEnded) return;

        currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:D2}:{seconds:D2}";

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isTimerActive = false;
            TriggerLose();
        }
    }

    public void TriggerWin()
    {
        if (hasEnded) return;

        hasEnded = true;
        isTimerActive = false;

        int mode = PlayerPrefs.GetInt("GameMode", 1);
        if (mode == 1)
        {
            currentScore = Mathf.CeilToInt(currentTime);
            SaveAndDisplayScore(currentScore);
        }
        else
        {
            currentScore = 0;
            currentScoreText.text = "";
            highScoreText.text = "";
        }

        canvasClear.SetActive(true);
    }

    void SaveAndDisplayScore(int score)
    {
        int highScore = PlayerPrefs.GetInt(highScoreKey, 0);

        if (score > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save();
        }

        currentScoreText.text = "Điểm: " + score;
        highScoreText.text = "Kỷ lục: " + PlayerPrefs.GetInt(highScoreKey, 0);
    }

    void TriggerLose()
    {
        hasEnded = true;
        canvasFail.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void AddExtraTime(float amount)
    {
        if (!hasEnded && isTimerActive)
        {
            currentTime += amount;
        }
    }

    public void AddTimeButton()
    {
        AddExtraTime(extraTimeAmount);
    }
}
