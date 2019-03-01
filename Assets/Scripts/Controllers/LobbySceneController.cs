using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    #region General
    [Header("Canvas")]
    public GameObject uiCanvas;
    public GameObject creditsCanvas;
    #endregion
    
    private IEnumerator Start()
    {
        uiCanvas.SetActive(false);
        creditsCanvas.SetActive(false);

        while(GameManager.Instance.GetGameState() != GAME_STATE.LOBBY)
        {
            yield return null;
        }

        uiCanvas.SetActive(true);
    }

    IEnumerator WaitForGameState()
    {
        while(GameManager.Instance.GetGameState() != GAME_STATE.LOBBY)
        {
            yield return null;
        }

        uiCanvas.SetActive(false);

    }
}
