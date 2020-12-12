using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("End Level UI")]
    [SerializeField] private TextMeshProUGUI endLevelUIResultText;
    [SerializeField] private TextMeshProUGUI endLevelUIScoreText;
    [SerializeField] private TextMeshProUGUI endLevelUIHighscoreText;
    [SerializeField] private GameObject endLevelUIRoot;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI currentScoreText;

    public void Setup(GamePlayManager gamePlayManager)
    {
        gamePlayManager.OnScoreUpdate += ScoreUpdate;
        gamePlayManager.OnLevelEnd += CreateEndLevelUI;
    }

    private void ScoreUpdate(int newScore)
    {
        currentScoreText.text = newScore.ToString();
    }

    private void CreateEndLevelUI(bool result, int score, int hightScore)
    {
        if(result)
        {
            endLevelUIResultText.text = "VICTORY";
        }
        else
        {
            endLevelUIResultText.text = "YOU LOSE";
        }

        endLevelUIScoreText.text = "SCORE: " + score;
        endLevelUIHighscoreText.text = "BEST: " + hightScore;

        endLevelUIRoot.SetActive(true);
    }
}
