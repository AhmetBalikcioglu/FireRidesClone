using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePanel : Panel
{
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.AddListener(ShowPanel);
        EventManager.OnLevelFinish.AddListener(HidePanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.RemoveListener(ShowPanel);
        EventManager.OnLevelFinish.RemoveListener(HidePanel);
    }
}
