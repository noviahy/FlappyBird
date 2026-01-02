using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UIManager;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    public bool inputLocked = false;
    public SqawnPlayer player;
    public scoreManager scoreManager;
    public HUDUI HUDUI;

    private void Start()
    {
        CurrentState = GameState.Ready;
        ChangeState(CurrentState);
    }
    public enum GameState
    {
        Ready,
        Wait,
        Playing,
        Pause,
        GameOver
    }
    public GameState CurrentState { get; private set; }
    public GameState PreviousState { get; private set; }
    public void ChangeState(GameState state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
        switch (state)
        {
            case GameState.Ready:
                // Debug.Log("Ready");
                DeletePlayer();
                break;

            case GameState.Wait:
                scoreManager.Init();
                HUDUI.Init();
                // Debug.Log("Wait");
                break;

            case GameState.Playing:
                // Debug.Log("Playing");
                break;

            case GameState.Pause:
                // Debug.Log("Pause");
                break;

            case GameState.GameOver:
                // Debug.Log("GameOver");
                break;

            default:
                Debug.LogError($"Unhandled UIState: {CurrentState}");
                break;
        }
    }

    public void SetPreviousState()
    {
        ChangeState(PreviousState);
    }

    public void ResumePlayer()
    {
        player.Resumption();
    }

    public void SqawnPlayer()
    {
        player.SpawnPlayer();
    }

    public void DeletePlayer()
    {
        player.DeletePlayer();
    }
}
