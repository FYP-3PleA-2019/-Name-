using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{ 
    #region General
    [Header("General")]
    public Transform mainSpawnPoint;

    public TheEntity theEntity;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.GetCurrGameState() == GAME_STATE.PAUSED)
        {
            UIManager.Instance.transitionUI.PlayTransitionAnimation(1);
            Initialize();
        }
        else
        {
            StartCoroutine("WaitForState");
        }
    }

    void Initialize()
    {
        UIManager.Instance.controlUI.ShowCanvas();

        GameManager.Instance.player.transform.position = mainSpawnPoint.position;


        RoomManager.Instance.Initialize();

        theEntity.SetEntityState(ENTITY_STATE.IDLE);

        //call after count down or whatever
        GameManager.Instance.SetGameState(GAME_STATE.IN_GAME);
    }

    private IEnumerator WaitForState()
    {
        while (GameManager.Instance.GetCurrGameState() != GAME_STATE.PAUSED)
        {
            yield return null;
        }

        Initialize();
    }
}
