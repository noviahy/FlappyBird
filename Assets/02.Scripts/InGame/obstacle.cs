using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class obstacle : MonoBehaviour
{
    [SerializeField] GameObject up;
    [SerializeField] GameObject down;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] SpriteRenderer upHead;
    [SerializeField] SpriteRenderer downHead;
    [SerializeField] float minSpeed = 5f; // 초기 스피드
    [SerializeField] float maxSpeed = 7f; // 최대 스피드
    [SerializeField] float difficultyIncreaseRate = 0.4f; // 초당 난이도 증가
    float currentSpeed;
    public CoinManager coinManager;
    [SerializeField, Range(0f, 1f)]

    public float coinSqawnChange = 1;
    float topLimit; // 카메라 기준 위
    float bottomLimit; // 카메라 기준 아래
    float destroyX; // 장애물 삭제 위치
    float gapTop; // gap 위 y좌표
    float gapBottom; // gap 아래 y좌표
    bool counted = false;
    public player player;
    public scoreManager scoreManager;
    public GameManager gameManager;
    public UIManager uiManager;
    public coinItem coinItem;


    private void Start()
    {
        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect; // 카메라 기준 가로 길이의 반
        destroyX = -halfWidth * 1.1f; // 조금 더 길게 해서 사라지는 것이 보이지 않게 함
        currentSpeed = minSpeed;

    }
    private void Update()
    {
        if (gameManager.CurrentState == GameManager.GameState.Playing && uiManager.CurrentState != UIManager.UIState.Counting)
        {
            currentSpeed += difficultyIncreaseRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            transform.position += Vector3.left * currentSpeed * Time.deltaTime;// 이동

            if (transform.position.x < player.transform.position.x && !counted) // x좌표가 같을 때
            {
                scoreManager.addScore(); // 점수 추가
                counted = true;
            }

            checkDestroy(); // 위치 확인 후 삭제
        }

        // 게임 오버 후 메인으로 돌아갈 시 오브젝트 삭제
        else if (gameManager.CurrentState == GameManager.GameState.Ready)
        {
            Destroy(gameObject);
        }
    }
    void setHeight(GameObject obj, float height)
    {
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>(); // collider 가져오기
        if (col != null)
        {
            // y축만 목표 높이에 맞춤
            col.size = new Vector2(col.size.x, height);
            col.offset = Vector2.zero;
        }

        // Sprite는 그대로 두거나, Tiled 모드라면 size만 height에 맞춰 늘림
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null && sr.drawMode == SpriteDrawMode.Tiled)
        {
            sr.size = new Vector2(sr.size.x, height);
            sr.transform.localScale = Vector3.one; // scale 고정
        }
    }

    public void init(float center, float size)
    {
        if (UnityEngine.Random.value <= coinSqawnChange) { spawnCoin(center); }

        topLimit = Camera.main.orthographicSize;
        bottomLimit = -Camera.main.orthographicSize;

        gapTop = center + size / 2;
        gapBottom = center - size / 2;

        up.transform.localPosition = new Vector3(0f, gapTop + (topLimit - gapTop) / 2, 0f); // up의 중심 설정
        down.transform.localPosition = new Vector3(0f, gapBottom - (gapBottom - bottomLimit) / 2, 0f); // down의 중심 설정

        setHeight(up, topLimit - gapTop);
        setHeight(down, gapBottom - bottomLimit);

        SetHeadPosition(upHead, gapTop, true);
        SetHeadPosition(downHead, gapBottom, false);
    }
    void checkDestroy()
    {
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

    void spawnCoin(float centerY)
    {
        GameObject coi = Instantiate(coinPrefab, transform);
        coinItem coin = coi.GetComponent<coinItem>();

        coin.coinManager = coinManager;
        coi.transform.localPosition = new Vector3(0f, centerY, 0f);
    }

    void SetHeadPosition(SpriteRenderer head, float Height, bool isUp)
    {
        float headHalf = head.sprite.bounds.size.y / 2f;
        float y = isUp ? Height + headHalf : Height - headHalf;
        head.transform.localPosition = new Vector3(0f, y - head.transform.parent.localPosition.y, 0f);
    }
}
