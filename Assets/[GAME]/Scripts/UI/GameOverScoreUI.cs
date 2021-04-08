using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScoreUI : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _highScoreText;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnEndScoreUpdate.AddListener(EndScoreTextUpdate);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnEndScoreUpdate.RemoveListener(EndScoreTextUpdate);
    }

    private void EndScoreTextUpdate()
    {
        _scoreText.text = "Your Score: " + ScoreManager.Instance.score.ToString("0.00");
        _highScoreText.text = "Highest Score: " + PlayerPrefs.GetFloat("HighScore").ToString("0.00");
    }
}
