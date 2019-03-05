using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    #region General
    [Header("Canvas")]
    public GameObject creditsCanvas;
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
    }
}
