using System.Collections;
using TMPro;
using UnityEngine;


public class obstacleSpawner : MonoBehaviour
{
    [SerializeField] float gapSize; // gap 크기
    [SerializeField] GameObject obstacles;
    float spawnX; // 삭제 위치
    float gapCenterY; // gap 중심
    float spawnInterval; // 생성 시간
    public player player;
    public scoreManager scoreManager;
    public GameManager gameManager;
    public UIManager uiManager;
    public CoinManager coinManager;

    [SerializeField] float minSpawnInterval = 0.5f; // 최소 스폰 간격
    [SerializeField] float maxSpawnInterval = 4f; // 초기 스폰 간격
    [SerializeField] float difficultyIncreaseRate = 0.2f; // 초당 난이도 증가

    private void Start()
    {
        spawnInterval = maxSpawnInterval;
        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        spawnX = halfWidth * 1.1f;
    }

    public void startSqawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() // 반복
    {
        float timerCounter = maxSpawnInterval;
        // while문에 해당하지 않으면 yield까지 가지 못하고 반복문이 끝남
        while (gameManager.CurrentState != GameManager.GameState.GameOver && gameManager.CurrentState != GameManager.GameState.Ready) // 게임이 실행 중일때
        {
            if (gameManager.CurrentState != GameManager.GameState.Pause && uiManager.CurrentState == UIManager.UIState.InGame)
            {
                timerCounter += Time.deltaTime;

                spawnInterval -= difficultyIncreaseRate * Time.deltaTime;
                spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);
            }

            if (timerCounter >= spawnInterval)
            {
                spawn();
                timerCounter = 0f;
            }
            yield return null; // 다음 프레임까지 기다렸다 다시 실행
        }
    }

    void spawn()
    {
        // 랜덤 gap 중심 계산
        gapCenterY = Random.Range(-Camera.main.orthographicSize + gapSize, Camera.main.orthographicSize - gapSize * 1 / 2);
        // 프리팹 생성
        GameObject obj = Instantiate(obstacles);
        // obstacle 컴포넌트 가져오기
        obstacle obsComp = obj.GetComponent<obstacle>();
        // player 참조 주입
        obsComp.player = player;
        // scoreManager 참조 주입
        obsComp.scoreManager = scoreManager;
        // active 참조 주입
        obsComp.gameManager = gameManager;
        // UImanager 참조 주입
        obsComp.uiManager = uiManager;
        // coinManager 참조 주입
        obsComp.coinManager = coinManager;

        // 생성 위치 설정
        obj.transform.position = new Vector3(spawnX, 0f, 0f);
        // 초기화
        obsComp.init(gapCenterY, gapSize);

    }
}
