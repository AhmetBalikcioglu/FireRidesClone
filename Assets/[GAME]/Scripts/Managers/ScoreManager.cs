using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public float score;
    private float _playerHighestPosZ;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        score = 0;
        _playerHighestPosZ = 0;
        EventManager.OnLevelFinish.AddListener(EndGameScore);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnLevelFinish.RemoveListener(EndGameScore);
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isGameStarted)
            return;
        ScoreUpdate();
    }
    private void ScoreUpdate()
    {
        if (_playerHighestPosZ > CharacterManager.Instance.Player.transform.position.z)
            return;
        score += CharacterManager.Instance.Player.Rigidbody.velocity.z * Time.fixedDeltaTime * 0.1f;
        _playerHighestPosZ = CharacterManager.Instance.Player.transform.position.z;
        EventManager.OnScoreUIUpdate.Invoke();
    }

    private void EndGameScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            float highestScore = PlayerPrefs.GetFloat("HighScore");
            if (score > highestScore)
            {
                PlayerPrefs.SetFloat("HighScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
        EventManager.OnEndScoreUpdate.Invoke();
    }
}