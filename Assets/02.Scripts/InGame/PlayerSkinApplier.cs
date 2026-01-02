using System.Collections;
using UnityEngine;

public class PlayerSkinApplier : MonoBehaviour
{
    public SpriteRenderer playerRenderer;
    public Sprite[] skins;
    public Sprite[] frames;
    private Coroutine specialCoroutinePlayer;
    int currentFramePlayer = 0;
    public float fps = 10f;

    public void SelectSpecial() // Special Player 스킨 사용
    {
        if (specialCoroutinePlayer != null) return;

        specialCoroutinePlayer = StartCoroutine(PlayerGif());
    }
    public void StopSpecialPlayer()
    {
        if (specialCoroutinePlayer == null) return;

        StopCoroutine(specialCoroutinePlayer);
        specialCoroutinePlayer = null;
    }
    IEnumerator PlayerGif() // Special 스킨 코루틴
    {
        while (true)
        {
            currentFramePlayer = (currentFramePlayer + 1) % frames.Length;
            playerRenderer.sprite = frames[currentFramePlayer];
            yield return new WaitForSeconds(1f / fps);
        }
    }
    public void insertSkin(int skinID) // Player 이미지 설정 - player코드에서 호출
    {
        if (skinID >= skins.Length)
        {
            SelectSpecial();
            return;
        }
        playerRenderer.sprite = skins[skinID];
        Debug.Log("InsertSkin: " + skinID);
        StopSpecialPlayer();
    }

}
