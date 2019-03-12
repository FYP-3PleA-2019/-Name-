using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    #region General
    [Header("General")]
    public Transform mainSpawnPoint;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.GetCurrGameState() == GAME_STATE.IN_GAME)
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

        RoomManager.Instance.Initialize();
    }

    private IEnumerator WaitForState()
    {
        while (GameManager.Instance.GetCurrGameState() != GAME_STATE.IN_GAME)
        {
            yield return null;
        }

        Initialize();
    }
}
