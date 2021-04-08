using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private bool _isControllable = true;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnLevelFinish.AddListener(() => _isControllable = false);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnLevelFinish.RemoveListener(() => _isControllable = false);

    }

    private void Update()
    {
        if (!_isControllable)
            return;
        GetMouseInput();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance.isGameStarted)
                GameManager.Instance.StartGame();
            else
                EventManager.OnMouseDown.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            EventManager.OnMouseUp.Invoke();
        }
    }
}