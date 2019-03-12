using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    #region General
    [Header("General")]
    public GameObject creditsCanvas;
    public Transform mainSpawnPoint;
    public Transform shopSpawnPoint;
    #endregion

    private void Start()
    {
        if (GameManager.Instance.GetCurrGameState() == GAME_STATE.LOBBY)
        {
            Initialize();
            UIManager.Instance.transitionUI.PlayTransitionAnimation(1);
        }
        else
        {
            UIManager.Instance.controlUI.HideCanvas();
            creditsCanvas.SetActive(false);

            StartCoroutine("WaitForState");
        }
    }

    void Initialize()
    {
        UIManager.Instance.controlUI.ShowCanvas();
        creditsCanvas.SetActive(true);

        if(GameManager.Instance.GetPrevGameState() == GAME_STATE.SHOP)
            GameManager.Instance.player.transform.position = shopSpawnPoint.position;

        else
            GameManager.Instance.player.transform.position = mainSpawnPoint.position;
    }

    private IEnumerator WaitForState()
    {
        while (GameManager.Instance.GetCurrGameState() != GAME_STATE.LOBBY)
        {
            yield return null;
        }

        Initialize();
    }
}
