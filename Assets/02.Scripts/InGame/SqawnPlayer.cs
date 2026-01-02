using System.Collections;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class SqawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject currentPlayer;
    public obstacleSpawner sqawner;
    public GameManager gameManager;
    public UIManager uiManager;
    public InputManager inputManager;
    public coinItem coinItem;
    public PlayerSkinUI playerSkinUI;
    public PlayerSkinApplier playerSkinApplier;
    int skinID;
    public void SpawnPlayer()
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);

        currentPlayer = Instantiate(playerPrefab, new Vector2(-6f, 0f), Quaternion.identity); // 현제 player의 위치

        player playerScript = currentPlayer.GetComponent<player>();
        playerScript.sqawner = sqawner;
        playerScript.gameManager = gameManager;
        playerScript.UIManager = uiManager;

        inputManager.player = playerScript; // inputManager와 현제 player 연결 (중요!!!)

        playerScript.InitPlayer();

        // 이걸 연결 안 해줘서 또 또또또또 수정을... 의심될때 해볼걸 어떻게하는지 몰라서 안 했더니.. 익윽엑
        PlayerSkinApplier skinApplier = currentPlayer.GetComponent<PlayerSkinApplier>();
        skinID = playerSkinUI.getID();
        StartCoroutine(setSkin(skinApplier, skinID));

    }

    IEnumerator setSkin(PlayerSkinApplier applier, int skinID)
    {
        yield return null;
        applier.insertSkin(skinID);
    }

    public void DeletePlayer() // 삭제
    {
        if (currentPlayer != null)
        {
            playerSkinApplier.StopSpecialPlayer();
            Destroy(currentPlayer);
            inputManager.player = null; // inputManager과의 연결 해제
            currentPlayer = null; // currentPlayer에서 삭제
        }
    }

    public void Resumption() // 재개
    {
        if (currentPlayer != null)
            currentPlayer.GetComponent<player>().Resumption();
    }
}
