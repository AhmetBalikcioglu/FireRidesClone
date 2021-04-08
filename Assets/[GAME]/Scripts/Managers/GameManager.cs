using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public bool isGameStarted;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        EventManager.OnGameRestart.AddListener(GameRestart);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        EventManager.OnGameRestart.RemoveListener(GameRestart);
    }

    public void StartGame()
    {
        if (isGameStarted)
            return;

        EventManager.OnGameStart.Invoke();
        isGameStarted = true;
    }

    public void GameEnd()
    {
        if (!isGameStarted)
            return;

        EventManager.OnLevelFinish.Invoke();
        isGameStarted = false;
    }

    private void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}