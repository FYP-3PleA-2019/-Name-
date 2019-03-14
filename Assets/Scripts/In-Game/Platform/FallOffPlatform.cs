using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffPlatform : MonoBehaviour
{
    public GameObject platform;
    bool canLoad;

    private void Start()
    {
        canLoad = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (platform.GetComponent<MovingPlatform>().isGrounded == false && canLoad)
            {
                PlayerDetected();
            }
        }
    }

    void PlayerDetected()
    {
        canLoad = false;
        UIManager.Instance.transitionUI.PlayTransitionAnimation(0);
        GameManager.Instance.SetGameState(GAME_STATE.LOBBY);
        CustomSceneManager.Instance.LoadSceneWait(GAME_SCENE.LOBBY_SCENE, 1.5f);
    }
}
