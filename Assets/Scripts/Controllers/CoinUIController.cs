using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUIController : MonoBehaviour
{
    private Canvas coinCanvas;
    private Text coinText;

    private void Start()
    {
        coinCanvas = GetComponent<Canvas>();
        coinText = GetComponentsInChildren<Text>()[0];
    }

    public void UpdateCoinUI() //Update coin UI
    {
        coinText.text = "" + GameManager.Instance.Coins;
    }

    public void EnableCanvas()
    {
        coinCanvas.enabled = true;
    }

    public void DisableCanvas()
    {
        coinCanvas.enabled = false;
    }
}
