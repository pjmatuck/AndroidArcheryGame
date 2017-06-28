using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    public static int Points { get; set; }
    public static int AvailableArrows { get; set; }

    public Image[] arrowImage;
    public Text textPoints;
    public Text textTime;
    public float totalTime;
    public GameObject pausePanel, objectivePanel, gameOverPanel, fadeInBackPanel;

    private int totalArrows;
    private float availableTime;
    //private bool pauseGame;

    private GameManager GM;

    void Start()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
        availableTime = totalTime;
        textTime.text = availableTime.ToString();
        totalArrows = arrowImage.Length;

        Debug.Log(this.GetType().Name + ".cs State: " + GameManager.gameState);
        Debug.Log(this.GetType().Name + ".cs GM: " + GM);

        AvailableArrows = 10;

        SetupArrows(totalArrows);
    }


    void Update()
    {
        if (GameManager.gameState == GameState.PRESENTOBJECTIVE)
        {
            objectivePanel.SetActive(true);
        }

        if (GameManager.gameState == GameState.GAME)
        {
            if (pausePanel.activeSelf == true)
            {
                pausePanel.SetActive(false);
            }

            if (objectivePanel.activeSelf == true)
            {
                objectivePanel.SetActive(false);
            }

            availableTime -= Time.deltaTime;
            textTime.text = availableTime.ToString("0");

            textPoints.text = Points.ToString();

            if (availableTime <= 0)
            {
                GM.SetGameState(GameState.GAMEOVER);
                gameOverPanel.SetActive(true);
            }

            if (AvailableArrows <= 0)
            {
                GM.SetGameState(GameState.GAMEOVER);
                gameOverPanel.SetActive(true);
            } else
            {
                UpdateUIArrows();
            }

        }

        if (GameManager.gameState == GameState.PAUSE)
        {
            pausePanel.SetActive(true);
        }
    }

    public void HandleOnStateChange()
    {
        Debug.Log(this.GetType().Name + ".cs State changed:" + GameManager.gameState);
    }

    private void SetupArrows(int totalArrows)
    {
        for (int i = 0; i < arrowImage.Length; i++)
        {
            if (totalArrows > 0)
            {
                arrowImage[i].enabled = true;
                totalArrows--;
            } else
            {
                arrowImage[i].enabled = false;
            }
        }
    }

    private void UpdateUIArrows()
    {
        arrowImage[AvailableArrows-1].enabled = false;
    }

    public void ShowPauseScreen()
    {
        Debug.Log("Show Menu");
        GM.SetGameState(GameState.PAUSE);
    }

    public void ClosePauseScreen()
    {
        GM.SetGameState(GameState.GAME);
    }

    public void StartGame()
    {
        GM.SetGameState(GameState.GAME);
        Debug.Log("Start Game");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("FirstLevel");
        Debug.Log("Restart Game");
    }
}
