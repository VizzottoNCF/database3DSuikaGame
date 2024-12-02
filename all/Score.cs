using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]private int score = 0;
    public TMP_Text scoreText;
    public TMP_InputField userField;
    public TMP_Text user;
    public Button Save;

    public ScoreDataBank ScoreDatabank;

    private void Start()
    {
        userField.enabled = false;
        Save.enabled = false;
        ScoreDatabank = this.GetComponent<ScoreDataBank>();
    }
    void Update()
    {
        scoreText.text = "SCORE: \n" + score.ToString("D4");


        if (userField.enabled && user.text != "")
        {
            Save.enabled = true;
        }
    }

    public void rf_AddToScore(int _value)
    {
        score += _value;
    }

    public void rf_OnGameEnd()
    {
        userField.enabled = true;
    }

    public void rf_OnSavePressed()
    {
        ScoreDatabank.insertPlayer(user.text);
        ScoreDatabank.checkForHighScore(user.text, score);
    }

    public void rf_OnPlayAgainPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
