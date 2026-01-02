using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager UIManager;
    public player player;
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) // Input.GetMouseButtonDown()은 옛날 Input Manager이다
        {
            if (gameManager.inputLocked) // UI와 같이 입력 받는 것을 방지
                return;

            if (player == null) // player가 없을 때
                return;

            if (gameManager.CurrentState != GameManager.GameState.Playing && gameManager.CurrentState != GameManager.GameState.Wait)
                return;

            player.OnJumpInput(); // 그 외에 OnJumpInput 호출
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame) // esc 버튼
        {
            if (UIManager.CurrentState == UIManager.UIState.Counting) return;
             
            if (gameManager.CurrentState == GameManager.GameState.Playing || gameManager.CurrentState == GameManager.GameState.Wait) // 게임 진행 or wait시
            {
                gameManager.ChangeState(GameManager.GameState.Pause); // isGamePause를 true로 변경
                UIManager.ChangeUI(UIManager.UIState.Pause);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
            }

            else if (gameManager.CurrentState == GameManager.GameState.Pause && UIManager.CurrentState == UIManager.UIState.Pause) // Pause시
            {
                if (gameManager.PreviousState == GameManager.GameState.Playing)
                    UIManager.ChangeUI(UIManager.UIState.Counting);
                if (gameManager.PreviousState == GameManager.GameState.Wait)
                    UIManager.ChangeUI(UIManager.UIState.InGame);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                gameManager.SetPreviousState();
            }

            else if (UIManager.CurrentState == UIManager.UIState.Setting || UIManager.CurrentState == UIManager.UIState.Character) // setting 혹은 character일때
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                UIManager.SetPreviousState();
            }

            else if (gameManager.CurrentState == GameManager.GameState.GameOver)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                UIManager.ChangeUI(UIManager.UIState.Main);
                gameManager.ChangeState(GameManager.GameState.Ready);
                AudioManager.Instance.StopBgm();
            }

        }
    }
}
