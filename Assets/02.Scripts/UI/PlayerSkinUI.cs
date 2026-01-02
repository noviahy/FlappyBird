using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkinUI : MonoBehaviour
{
    public UIManager manager;
    public Image uiImage;
    public Sprite[] skins;
    public Sprite[] frames;
    public int index = 0;
    public int selectedSkinID = 0;
    public int currentSkinID = 0;
    public float fps = 10f;
    int currentFrameUI = 0;
    private Coroutine specialCoroutineUI;
    public bool isSpecial = false;
    private void Start()
    {
        if (currentSkinID < skins.Length)
            SetImage(currentSkinID);
        else
            UISpecial();
    }
    public void leftBT() // 왼쪽 버튼
    {
        if (isSpecial)
        {
            StopSpecialUI();
            isSpecial = false;
            selectedSkinID--;
        }
        else
        {
            index--;
            selectedSkinID = index;

        }
        SetImage(index);
        // Debug.Log(index);
    }
    public void RightBT() // 오른쪽 버튼
    {
        if (isSpecial) return;

        if (index >= skins.Length - 1)
        {
            isSpecial = true;
            UISpecial();
            selectedSkinID++;
            // Debug.Log(index);
            return;
        }

        index++;
        selectedSkinID = index;
        SetImage(index);
        // Debug.Log(index);
    }
    public void SetImage(int index) // UI 이미지 설정
    {
        uiImage.sprite = skins[index];
    }
    public bool IsSelected() // UI와 Player가 같은 이미지인지 확인 - 방향 버튼에서 호출
    {
        return selectedSkinID == currentSkinID;
    }
    public void SelectSkin() // 스킨 선택 - select 버튼에서 호출
    {
        currentSkinID = selectedSkinID;
    }
    public void UISpecial() // UI에 Special 이미지 띄우기
    {
        if (specialCoroutineUI != null) return;

        specialCoroutineUI = StartCoroutine(ImageGif());
    }
    public void StopSpecialUI() // UI Speical 이미지 끄기
    {
        if (specialCoroutineUI == null) return;

        StopCoroutine(specialCoroutineUI);
        specialCoroutineUI = null;
    }

    IEnumerator ImageGif() // Special UI 코루틴
    {
        while (true)
        {
            if (manager.CurrentState != UIManager.UIState.Character)
            {
                currentFrameUI = 0;
                StopSpecialUI();
                yield break;
            }

            currentFrameUI = (currentFrameUI + 1) % frames.Length;
            uiImage.sprite = frames[currentFrameUI];
            yield return new WaitForSeconds(1f / fps);
        }
    }
    public int getID()
    {
        return currentSkinID;
    }

}
