using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGamePanel : Panel
{
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.AddListener(HidePanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.RemoveListener(HidePanel);
    }
}
