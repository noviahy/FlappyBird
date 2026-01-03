using TMPro;
using UnityEngine;

public class HUDUI : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] UIManager UIManager;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timeText;

    RectTransform rect;
    Vector2 originalPos;
    Vector2 originalSize;
    Vector2 originalMin;
    Vector2 originalMax;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition; // 기본 위치
        originalSize = rect.localScale; // 기본 크기
        originalMax = rect.anchorMax;
        originalMin = rect.anchorMin;
    }

    public void moveToCenter()
    {
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);

        rect.anchoredPosition = Vector2.zero; // 중앙

        scoreText.alignment = TextAlignmentOptions.Center; // 글 꼴을 중앙으로
        timeText.alignment = TextAlignmentOptions.Center;
    }

    public void Init()
    {
        rect.anchorMin = originalMin;
        rect.anchorMax = originalMax;

        // 기본으로 돌아옴
        rect.anchoredPosition = originalPos;
        rect.localScale = originalSize;
        // 글꼴을 왼쪽으로
        scoreText.alignment = TextAlignmentOptions.Left;
        timeText.alignment = TextAlignmentOptions.Left;
    }
}
