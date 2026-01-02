using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    [SerializeField] SkinShopManager skinShopManager;
    public int coin = 10;
    void Update()
    {
        coinText.text = "Apple: " + coin.ToString();
    }

    public void addCoin()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.coin);
        coin++;
        if (coin >= skinShopManager.price && !skinShopManager.isBuy)
            skinShopManager.ChangeState(SkinShopManager.ShopState.CanBuy);
        if (coin < skinShopManager.price && !skinShopManager.isBuy)
            skinShopManager.ChangeState(SkinShopManager.ShopState.CantBuy);
    }
}
