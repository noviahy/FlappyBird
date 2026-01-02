using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float jumpPower;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float FallSpeed;
    float cur_value;
    float pre_value;
    float cur_Angle;
    float originalGravity;
    float maxJumpY;
    Vector2 originalPos;
    Vector2 savedVelocity;
    float savedAngle;
    bool jumpApplied = false;
    bool jumpRequested = false;
    public obstacleSpawner sqawner;
    public GameManager gameManager;
    public UIManager UIManager;
    public CoinManager CoinManager;

    public void InitPlayer()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        originalPos = transform.position;
        maxJumpY = Camera.main.orthographicSize;
        rb.gravityScale = 0; // 처음 중력 = 0
    }

    private void Update()
    {
        if (gameManager.CurrentState == GameManager.GameState.Playing && UIManager.CurrentState != UIManager.UIState.Counting)
        {
            cur_value = rb.linearVelocity.y; // 현재 속도


            // 하강 가속도 추가
            if (cur_value <= 0 && pre_value >= 0)
            {
                startFallBoost();
            }

            // 상승시 45도 각도 고정
            if (cur_value > 0)
            {
                cur_Angle = 45f;
            }

            // 하강시 y속도 -> 각도 선형 보간을 통해 45도 ~ -45도 회전
            if (cur_value < 0)
            {
                cur_Angle = Mathf.Lerp(45f, -45f, cur_value / maxFallSpeed);
            }

            // 현재 회전과 목표 회전을 부드럽게 보간
            float smoothFactor = 10f; // 숫자를 높일수록 빠르게 회전
            float angle = Mathf.LerpAngle(transform.eulerAngles.z, cur_Angle, smoothFactor * Time.deltaTime);

            // 회전 적용
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            pre_value = cur_value; // 현제 속도를 이전 속도 변수에 넣음
        }

        if ((gameManager.CurrentState == GameManager.GameState.Pause
            && gameManager.PreviousState == GameManager.GameState.Playing)
            || UIManager.CurrentState == UIManager.UIState.Counting)
        {
            Paused();
        }
    }
    // 결정된 행동을 실행
    private void FixedUpdate()
    {
        // masFallSpped보다 하강속도가 빨라기면 masFallSpeed로 속도 고정
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }

        if (jumpRequested)
        {
            HandleJump();
            jumpRequested = false;
        }
    }

    // 입력은 외부 이벤트에서
    public void OnJumpInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) // UI클릭시 입력 받지 않음
            return;

        // Debug.Log("Jump!");

        jumpRequested = true;
    }
    // 상태 변화
    void HandleJump()
    {
        if (!jumpApplied) // 최초 1회 실행
        {
            gameManager.ChangeState(GameManager.GameState.Playing);// 상태: Playing
            sqawner.startSqawn();

            rb.gravityScale = originalGravity;
            jumpApplied = true; // true 설정으로 다시 실행되지 않는다
        }
        Jump();
    }
    public void Jump()
    {
        if (transform.position.y <= maxJumpY)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jump);
            // Debug.Log("Jump!");
            // gravity를 바꾸는 것 보다 velocity를 사용해 위로 힘을 주는게 더 자연스러움
            rb.linearVelocity = Vector2.up * jumpPower;
        }
    }

    void startFallBoost()
    {
        rb.linearVelocity = Vector2.down * FallSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameManager.CurrentState != GameManager.GameState.Playing) return;

        if (other.CompareTag("Obstacle") || other.CompareTag("Floor"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.hit);
            gameManager.ChangeState(GameManager.GameState.GameOver); // 상태: GameOver
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero; // 플레이어의 현재 이동 속도를 전부 0으로 만들어서 즉시 멈추게함
        }
    }

    void Paused()
    {
        savedVelocity = rb.linearVelocity;
        savedAngle = cur_Angle;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    public void Resumption()
    {
        rb.linearVelocity = savedVelocity;
        rb.gravityScale = originalGravity;
        cur_Angle = savedAngle;
    }
}