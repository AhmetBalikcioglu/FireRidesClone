using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : Button
{
    protected override void OnEnable()
    {
        base.OnEnable();
        onClick.AddListener(RestartLevel);
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        onClick.RemoveListener(RestartLevel);
    }

    public void RestartLevel()
    {
        EventManager.OnGameRestart.Invoke();
    }
}
