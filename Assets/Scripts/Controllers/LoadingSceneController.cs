using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneController : MonoBehaviour
{
    private IEnumerator Start()
    {
        UIManager.Instance.controlUI.HideCanvas();

        while(!GameManager.Instance.GetIsReady())
        {
            yield return null;
        }

        GameManager.Instance.SetGameState(GAME_STATE.MAIN_MENU);
        CustomSceneManager.Instance.LoadScene(GAME_SCENE.LOBBY_SCENE);
    }
}
