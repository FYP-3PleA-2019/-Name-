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
        if (GameManager.Instance.currGameState != GAME_STATE.MAIN_MENU)
        {
            GameManager.Instance.player.controller.PlayTeleportAnimation();
        }

        if (GameManager.Instance.GetCurrGameState() == GAME_STATE.LOBBY)
        {
            Initialize();
            UIManager.Instance.transitionUI.PlayTransitionAnimation(1);
        }
        else
        {
            UIManager.Instance.controlUI.HideCanvas();
            UIManager.Instance.coinUI.DisableCanvas();
            creditsCanvas.SetActive(false);

            StartCoroutine("WaitForState");
        }
    }

    void Initialize()
    {
        StartCoroutine(EnableCanvases());
        creditsCanvas.SetActive(true);

        if (GameManager.Instance.GetPrevGameState() == GAME_STATE.SHOP)
            GameManager.Instance.player.transform.position = new Vector2(shopSpawnPoint.position.x + .1f, shopSpawnPoint.position.y + 0.45f);

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

    IEnumerator EnableCanvases()
    {
        yield return new WaitForSeconds(.75f);
        UIManager.Instance.controlUI.ShowCanvas();
        UIManager.Instance.coinUI.EnableCanvas();
    }
}
