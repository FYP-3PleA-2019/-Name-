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
    
    private IEnumerator Start()
    {
        UIManager.Instance.controlUI.HideCanvas();
        creditsCanvas.SetActive(false);

        while(GameManager.Instance.GetGameState() != GAME_STATE.LOBBY)
        {
            yield return null;
        }

        UIManager.Instance.controlUI.ShowCanvas();
        creditsCanvas.SetActive(true);

        TeleportPlayer();
    }

    void TeleportPlayer()
    {
        if(CustomSceneManager.Instance.GetPrevScene() == GAME_SCENE.SHOP_SCENE)
        {
            GameManager.Instance.player.transform.position = shopSpawnPoint.position;
        }
        else
        {
            GameManager.Instance.player.transform.position = mainSpawnPoint.position;
        }
    }
}
