using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIButtonHandler : MonoBehaviour
{ 
    [SerializeField] Button start;
    [SerializeField] Button character;
    [SerializeField] Button setting;
    [SerializeField] Button left;
    [SerializeField] Button right;
    [SerializeField] Button select;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    public GameManager gameManager;
    public UIManager UIManager;
    public PlayerSkinUI playerSkinUI;
    public SkinShopManager skinShopManager;
    public TextMeshProUGUI buttonText;
    bool inputLocked;
    bool isMute = false;

    private void Start()
    {
        select.interactable = false;

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        AudioManager.Instance.bgmSource.volume = bgmSlider.value;
        AudioManager.Instance.sfxSource.volume = sfxSlider.value;

        buttonText = select.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetBGMVolume(float value)
    {
        if (isMute) return;

        AudioManager.Instance.bgmSource.volume = value; // 0~1
    }

    public void SetSFXVolume(float value)
    {
        if (isMute) return;

        AudioManager.Instance.sfxSource.volume = value; // 0~1
    }

    public void StartClicked() // 시작 버튼
    {
        UIManager.ChangeUI(UIManager.UIState.InGame);
        gameManager.inputLocked = true; // Lock
        gameManager.ChangeState(GameManager.GameState.Wait);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        AudioManager.Instance.PlayBGM();
        StartCoroutine(StartGameRoutine());
    }

    public void MMClicked() // Main Menu 버튼
    {
        UIManager.ChangeUI(UIManager.UIState.Main);
        gameManager.ChangeState(GameManager.GameState.Ready);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        AudioManager.Instance.StopBgm();
    }

    public void PauseClicked() // 일시 정지 버튼
    {
        if (gameManager.CurrentState == GameManager.GameState.Playing || gameManager.CurrentState == GameManager.GameState.Wait) // isGamePause가 false 일때
        {
            gameManager.ChangeState(GameManager.GameState.Pause);// isGamePause를 true로 변경
            UIManager.ChangeUI(UIManager.UIState.Pause);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        }

        else if (gameManager.CurrentState == GameManager.GameState.Pause) // Pause 상태일때 
        {
            if (gameManager.PreviousState == GameManager.GameState.Playing)
                UIManager.ChangeUI(UIManager.UIState.Counting);

            if (gameManager.PreviousState == GameManager.GameState.Wait)
                UIManager.ChangeUI(UIManager.UIState.InGame);
            gameManager.SetPreviousState();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        }
    }

    public void SettingClicked() // setting 버튼 
    {
        UIManager.ChangeUI(UIManager.UIState.Setting);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public void BackCliked() // 뒤로가기
    {
        UIManager.SetPreviousState();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public void ContinueClicked() // continue 버튼
    {
        if (gameManager.PreviousState == GameManager.GameState.Playing) // 전 상태가 play일때만 resumePlayer 호출
            UIManager.ChangeUI(UIManager.UIState.Counting);
        if (gameManager.PreviousState == GameManager.GameState.Wait)
            UIManager.ChangeUI(UIManager.UIState.InGame);
        gameManager.SetPreviousState();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public void CharacterClicked() // 캐릭터창
    {
        UIManager.ChangeUI(UIManager.UIState.Character);
        SetButtonText();
        if (skinShopManager.canBuy()) // 디거그용 나중에 뺄거임
        {
            skinShopManager.ChangeState(SkinShopManager.ShopState.CanBuy);
        }
        if (playerSkinUI.isSpecial)
            playerSkinUI.UISpecial();
        SetButtonInteract();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public void LeftClicked() // 캐릭터창-왼쪽
    {
        if (playerSkinUI.index == 0) return;
        if (inputLocked) return;
        inputLocked = true;
        
        playerSkinUI.leftBT();
        select.interactable = !playerSkinUI.IsSelected();

        StartCoroutine(UnlockInput());
        SetButtonText();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public void RightClicked() // 캐릭터창-오른쪽
    {
        if (playerSkinUI.isSpecial) return;
        if (inputLocked) return;
        inputLocked = true;

        playerSkinUI.RightBT();
        SetButtonInteract();

        StartCoroutine(UnlockInput());
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public void SelectClicked() // 캐릭터창 - 선택
    {
        if (skinShopManager.CurrentState == SkinShopManager.ShopState.CanBuy)
        {
            skinShopManager.BuySkin();
            skinShopManager.ChangeState(SkinShopManager.ShopState.AlreadyBuy);
            SetButtonText();
            return;
        }
        playerSkinUI.SelectSkin();
        select.interactable = false;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public void ToggleMute(bool isOn)
    {
        if (isOn)
        {
            isMute = true;
            Debug.Log("true");
            AudioManager.Instance.bgmSource.volume = 0;
            AudioManager.Instance.sfxSource.volume = 0;
        }

        else
        {
            isMute = false;
            AudioManager.Instance.bgmSource.volume = bgmSlider.value;
            AudioManager.Instance.sfxSource.volume = sfxSlider.value;
        }
    }

    void SetButtonText()
    {
        if (playerSkinUI.isSpecial)
        {
            if (skinShopManager.CurrentState == SkinShopManager.ShopState.CantBuy)
                buttonText.text = "Need 10 Apples";

            if (skinShopManager.CurrentState == SkinShopManager.ShopState.CanBuy)
                buttonText.text = "Buy";

            if (skinShopManager.CurrentState == SkinShopManager.ShopState.AlreadyBuy)
                buttonText.text = "Select";
        }
        else
            buttonText.text = "Select";
    }

    void SetButtonInteract()
    {
        if (playerSkinUI.isSpecial)
        {
            if (skinShopManager.CurrentState == SkinShopManager.ShopState.CantBuy)
            {
                select.interactable = false;
                SetButtonText();
            }

            if (skinShopManager.CurrentState == SkinShopManager.ShopState.CanBuy)
            {
                select.interactable = true;
                SetButtonText();
            }
            if (skinShopManager.CurrentState == SkinShopManager.ShopState.AlreadyBuy)
            {
                select.interactable = !playerSkinUI.IsSelected();
                SetButtonText();
            }
        }
        else
        {
            select.interactable = !playerSkinUI.IsSelected();
            SetButtonText();
        }
    }

    IEnumerator StartGameRoutine()
    {
        while (Mouse.current.leftButton.isPressed)
            yield return null;

        gameManager.SqawnPlayer(); // player 스폰
        gameManager.inputLocked = false; // Lock 해제
    }
    IEnumerator UnlockInput()
    {
        yield return new WaitForSeconds(0.05f);
        inputLocked = false;
    }
}
