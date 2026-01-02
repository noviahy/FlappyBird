using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class coinItem : MonoBehaviour
{
    public CoinManager coinManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            coinManager.addCoin();
            Destroy(gameObject);
        }
    }
}
