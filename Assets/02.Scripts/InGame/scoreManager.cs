using UnityEngine;

public class scoreManager : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uiManager;
    public UIScore uiScore;

    float timer = 0f;
    int score = 0;
    int BestScore = 0;
    bool scorePrinted = false;

    void Update()
    {
        if (gameManager.CurrentState == GameManager.GameState.Playing && uiManager.CurrentState != UIManager.UIState.Counting) // 게임 실행시에만
            timer += Time.deltaTime; // 타이머


        else if (gameManager.CurrentState == GameManager.GameState.GameOver && !scorePrinted) // socrePrinted로 한번만 출력
        {
            if (BestScore < score) 
                BestScore = score;
            uiScore.outputBestScore();
            printScore();
        }
    }

    void printScore() // 확인용
    {
        scorePrinted = true;
    }

    public void addScore() // obstalce.cs에서 호출
    {
        score++;
    }

    //UIScore에서 호출
    public string getScore() { return score.ToString(); } 
    public string getTime() { return timer.ToString("F2"); }

    public string getBestScore() {return BestScore.ToString(); }

    public void Init()
    {
        timer = 0f;
        score = 0;
        scorePrinted = false;
    }
}
