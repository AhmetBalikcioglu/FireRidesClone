using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : Panel
{
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.AddListener(HidePanel);
        EventManager.OnLevelFinish.AddListener(ShowPanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.RemoveListener(HidePanel);
        EventManager.OnLevelFinish.RemoveListener(ShowPanel);
    }
}
