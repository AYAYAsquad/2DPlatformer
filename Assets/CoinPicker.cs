using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPicker : MonoBehaviour
{
    private float coin = 0;

    public TextMeshProUGUI textCoins;

    private void OnTriggerEnter2D(Collider2D other)
    {
    if(other.transform.tag == "Coin")
        {
            coin = coin + 1;
            textCoins.text = coin.ToString();
            Destroy(other.gameObject);
        }
    }
}
