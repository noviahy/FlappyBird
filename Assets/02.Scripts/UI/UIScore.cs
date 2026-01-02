using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] scoreManager scoreManager;
    void Update()
    {
        scoreText.text = "Score: " + scoreManager.getScore();
        timeText.text = "Time: " + scoreManager.getTime();
    }

    public void outputBestScore()
    {
        bestScoreText.text = "Best Score: "+ scoreManager.getBestScore();
    }
}
