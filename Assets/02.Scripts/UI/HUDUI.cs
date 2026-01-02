using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HUDUI : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] UIManager UIManager;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timeText;

    RectTransform rect;
    Vector2 originalPos;
    Vector2 originalSize;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition; // 기본 위치
        originalSize = rect.localScale; // 기본 크기
    }

    void Update()
    {
        if (GameManager.CurrentState == GameManager.GameState.GameOver) // 게임 오버 시
        {
            UIManager.ChangeUI(UIManager.UIState.GameOver);
            moveToCenter(); // 중앙으로 옮김
        }
    }

    public void moveToCenter()
    {
        rect.anchoredPosition = Vector2.zero; // 중앙
        rect.localScale = Vector2.one * 1.5f; // 1.5배
        scoreText.alignment = TextAlignmentOptions.Center; // 글 꼴을 중앙으로
        timeText.alignment = TextAlignmentOptions.Center;
    }

    public void Init()
    {
        // 기본으로 돌아옴
        rect.anchoredPosition = originalPos;
        rect.localScale = originalSize;
        // 글꼴을 왼쪽으로
        scoreText.alignment = TextAlignmentOptions.Left;
        timeText.alignment = TextAlignmentOptions.Left;
    }
}
