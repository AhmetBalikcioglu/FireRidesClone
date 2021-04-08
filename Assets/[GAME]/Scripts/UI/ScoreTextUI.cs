using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextUI : MonoBehaviour
{
    private Text _scoreText;
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        _scoreText = GetComponent<Text>(); 
        EventManager.OnScoreUIUpdate.AddListener(ScoreTextUpdate);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return; 
        EventManager.OnScoreUIUpdate.RemoveListener(ScoreTextUpdate);
    }

    private void ScoreTextUpdate()
    {
        _scoreText.text = ScoreManager.Instance.score.ToString("0.00");
    }

}
