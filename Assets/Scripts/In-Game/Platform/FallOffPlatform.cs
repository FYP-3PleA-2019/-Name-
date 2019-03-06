using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Static,
    Movable
}

public class FallOffPlatform : MonoBehaviour
{
    public GameObject platform;

    public PlatformType _platformType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_platformType == PlatformType.Movable)
            {
                if (platform.GetComponent<MovingPlatform>().isGrounded == false)
                {
                    GameManager.Instance.SetGameState(GAME_STATE.LOBBY);
                    CustomSceneManager.Instance.LoadSceneWait(GAME_SCENE.LOBBY_SCENE, 0.5f);
                }
            }
            else
            {
                GameManager.Instance.SetGameState(GAME_STATE.LOBBY);
                CustomSceneManager.Instance.LoadSceneWait(GAME_SCENE.LOBBY_SCENE, 0.5f);
            }
        }
    }
}
