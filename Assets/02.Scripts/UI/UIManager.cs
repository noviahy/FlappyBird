using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup mainUI;
    [SerializeField] CanvasGroup InGameUI;
    [SerializeField] CanvasGroup endUI;
    [SerializeField] CanvasGroup PauseUI;
    [SerializeField] CanvasGroup SettingUI;
    [SerializeField] CanvasGroup CharacterUI;
    [SerializeField] Button Setting;
    [SerializeField] TMP_Text CountText;
    public GameManager gameManager;
    public UIState CurrentState { get; private set; }  // 현재 UI 상태
    public UIState PreviousState { get; private set; } // 이전 UI 상태

    public enum UIState
    {
        Main,
        InGame,
        Pause,
        GameOver,
        Setting,
        Character,
        Counting
    }

    private void Start()
    {
        HideAll();
        CurrentState = UIState.Main;
        Show(mainUI);
    }

    void HideAll()
    {
        Hide(mainUI);
        Hide(InGameUI);
        Hide(endUI);
        Hide(PauseUI);
        Hide(SettingUI);
        Hide(CharacterUI);
    }

    public void Hide(CanvasGroup cg)
    {
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void Show(CanvasGroup cg)
    {
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void ChangeUI(UIState state)
    {
        PreviousState = CurrentState;
        CurrentState = state;

        HideAll(); // 여기서 모든 가능성 제거

        switch (state)
        {
            case UIState.Main:
                Show(mainUI);
                break;

            case UIState.InGame:
                // Debug.Log("InGame");
                Show(InGameUI);
                showPauseBT();
                break;

            case UIState.Pause:
                Debug.Log("Pause");
                Show(PauseUI);
                showPauseBT();
                Show(InGameUI);
                break;

            case UIState.GameOver:
                Show(InGameUI);
                hidePauseBT();
                Show(endUI);
                break;

            case UIState.Setting:
                Debug.Log("Setting");
                Show(SettingUI);
                hidePauseBT();
                break;

            case UIState.Character:
                Debug.Log("Character");
                Show(CharacterUI);
                break;

            case UIState.Counting:
                Debug.Log("Counting");
                StartCoroutine(PlayCountdown());
                break;

            default:
                Debug.LogError($"Unhandled UIState: {state}");
                break;
        }
    }
    public void SetPreviousState()
    {
        ChangeUI(PreviousState);
    }
    public void showPauseBT()
    {
        Setting.gameObject.SetActive(true);
    }

    public void hidePauseBT()
    {
        Setting.gameObject.SetActive(false);
    }
    // 카운트다운 전 상태가 Playing일때만 실행된다
    public IEnumerator PlayCountdown()
    {
        int timer = 3;
        CountText.gameObject.SetActive(true);
        for (int i = timer; i > 0; i--)
        {
            CountText.text = i.ToString();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.countDown);
            yield return new WaitForSeconds(1f); // 1초 대기
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.start);
        ChangeUI(UIState.InGame); // InGameUI로 자동 변경
        CountText.gameObject.SetActive(false);
        gameManager.ResumePlayer(); // 재개
    }
}
