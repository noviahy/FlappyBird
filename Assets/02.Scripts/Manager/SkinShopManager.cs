using UnityEngine;
using static GameManager;

public class SkinShopManager : MonoBehaviour
{
    public CoinManager CoinManager;
    public bool isBuy = false;
    public int price = 10;

    private void Start()
    {
        ChangeState(ShopState.CantBuy);
    }
    public ShopState CurrentState { get; private set; }
    public enum ShopState
    {
        CanBuy,
        CantBuy,
        AlreadyBuy
    }
    public bool isTenCoin() { return CoinManager.coin >= price; }
    public void ChangeState(ShopState state)
    {
        CurrentState = state;
        switch (state)
        {
            case ShopState.CantBuy:
                Debug.Log("Can't Buy");
                break;

            case ShopState.CanBuy:
                Debug.Log("Can Buy");
                break;

            case ShopState.AlreadyBuy:
                Debug.Log("Already Buy");
                isBuy = true;

                break;

            default:
                Debug.LogError($"Unhandled UIState: {CurrentState}");
                break;
        }
    }

    public void BuySkin()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buy);
        CoinManager.coin -= price;
    }
    public bool canBuy()
    {
        return CoinManager.coin >= price;
    }
}
